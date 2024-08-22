using CoreLib.DTO.Adjustment;
using CoreLib.DTO.Common.Dialog;
using CoreLib.DTO.Common.TrxHeader;
using CoreLib.DTO.Receiving;
using CoreLib.Interfaces;
using CoreLib.Inventory.Interfaces;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using Inventory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Modules.Inventory;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace InvWeb.Pages.Stores.Adjustment
{
    public class FormModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DetailsModel> _logger;
        private readonly IItemTrxServices itemTrxServices;
        private readonly IInvApprovalServices invApprovalServices;
        private readonly IItemServices _itemServices;
        private readonly IUomServices uomServices;
        private readonly DateServices dateServices;

        public FormModel(ILogger<DetailsModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            itemTrxServices = new ItemTrxServices(_context, _logger);
            invApprovalServices = new InvApprovalServices(_context, _logger);
            _itemServices = new ItemServices(context);
            uomServices = new UomServices(_context);
            dateServices = new DateServices();
            AdjustmentDetailsModel = new AdjustmentDetailsModel();
        }

        //public InvTrxHdr InvTrxHdr { get; set; }
        public AdjustmentDetailsModel AdjustmentDetailsModel { get; set; }

        public InvTrxDtl InvTrxDtl { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //AdjustmentDetailsModel.InvTrxHdr = itemTrxServices.GetInvTrxHdrsById((int)id)
            //                             .FirstOrDefault();


            AdjustmentDetailsModel.InvTrxHdr = await itemTrxServices.GetInvTrxHdrsByIdAsync((int)id);
            AdjustmentDetailsModel.InvTrxDtls = itemTrxServices.GetInvTrxDtlsById((int)id);
            var storeId = AdjustmentDetailsModel.InvTrxHdr.InvStoreId;


            if (AdjustmentDetailsModel.InvTrxHdr == null)
            {
                return NotFound();
            }

            AdjustmentDetailsModel.InvTrxDtls = itemTrxServices.GetInvTrxDtlsById((int)id);

            var storeAreas = _context.InvStoreAreas.Where(a => a.InvStoreId == AdjustmentDetailsModel.InvTrxHdr.InvStoreId).ToList();

            ViewData["InvItems"] = new SelectList(_itemServices.GetInvItemsSelectList().Include(i => i.InvCategory)
                                    .Select(x => new
                                    {
                                        Name = String.Format("({0}) {1} - {2} ",
                                        x.Code, x.InvCategory.Description, x.Description),
                                        Value = x.Id
                                    }), "Value", "Name", id);

            ViewData["HdrId"] = AdjustmentDetailsModel.InvTrxHdr.Id;
            ViewData["UomsList"] = new SelectList(uomServices.GetUomSelectList(), "Id", "uom");
            ViewData["DateToday"] = dateServices.GetCurrentDate().ToString("yyy-MM-dd");
            ViewData["Brands"] = new SelectList(_context.InvItemBrands, "Id", "Name");
            ViewData["Origins"] = new SelectList(_context.InvItemOrigins, "Id", "Name");
            ViewData["Operator"] = new SelectList(_context.InvTrxDtlOperators, "Id", "Operator");
            ViewData["StoreAreas"] = new SelectList(storeAreas, "Id", "Name");
            ViewData["DialogItems"] = ConvertItemsToDialogItems((List<InvItem>)_itemServices.GetInvItemsWithSteelSpecs());
            ViewData["StoreId"] = storeId;


            return Page();
        }


        public IEnumerable<DialogItems> ConvertItemsToDialogItems(List<InvItem> invItems)
        {
            List<DialogItems> dialogItems = new List<DialogItems>();

            foreach (InvItem item in invItems)
            {
                var itemspecs = "";
                if (item.InvItemSpec_Steel.Count > 0)
                {
                    var spec = item.InvItemSpec_Steel.First();

                    itemspecs = spec.SteelMainCat.Name + " " + spec.SteelMaterial.Name;
                }

                string remarkString = "";
                if (!String.IsNullOrEmpty(item.Remarks))
                {
                    remarkString = " - " + item.Remarks;
                }


                dialogItems.Add(new DialogItems
                {
                    Id = item.Id,
                    Name = item.Code + " - " + item.InvCategory.Description + " - " + item.Description,
                    Description = itemspecs + remarkString
                });
            }

            return dialogItems;
        }

        public bool CheckIfUserIsAdmin()
        {
            return User.IsInRole("Admin");
        }
    }
}
