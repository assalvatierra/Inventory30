using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Interfaces;
using Microsoft.Extensions.Logging;
using Modules.Inventory;
using CoreLib.DTO.Releasing;
using CoreLib.Interfaces;
using Inventory;
using System.Threading.Tasks;
using CoreLib.DTO.Receiving;
using System.Collections.Generic;
using CoreLib.DTO.Common.Dialog;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InvWeb.Pages.Stores.Receiving
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

            ReceivingDetailsModel = new ReceivingDetailsModel();
        }
        //public InvTrxHdr InvTrxHdr { get; set; }
        public ReceivingDetailsModel ReceivingDetailsModel { get; set; }
        public ReceivingItemDtlsCreateEditModel ItemDtlsCreateModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            ReceivingDetailsModel.InvTrxHdr = await itemTrxServices.GetInvTrxHdrsById((int)id)
                                         .FirstOrDefaultAsync();

            if (ReceivingDetailsModel.InvTrxHdr == null)
            {
                return NotFound();
            }

            ReceivingDetailsModel.InvTrxDtls = itemTrxServices.GetInvTrxDtlsById((int)id);

            //ReceivingDetailsModel.InvTrxApproval = invApprovalServices.GetExistingApproval((int)id);

            var storeAreas = _context.InvStoreAreas.Where(a => a.InvStoreId == ReceivingDetailsModel.InvTrxHdr.InvStoreId).ToList();

            ViewData["InvItems"] = new SelectList(_itemServices.GetInvItemsSelectList().Include(i => i.InvCategory)
                                    .Select(x => new
                                    {
                                        Name = String.Format("({0}) {1} - {2} ",
                                        x.Code, x.InvCategory.Description, x.Description),
                                        Value = x.Id
                                    }), "Value", "Name", id);

            ViewData["UomsList"] = new SelectList(uomServices.GetUomSelectList(), "Id", "uom");
            ViewData["DialogItems"] = ConvertItemsToDialogItems((List<InvItem>)_itemServices.GetInvItemsWithSteelSpecs());
            ViewData["DateToday"] = dateServices.GetCurrentDate().ToString("yyy-MM-dd");
            ViewData["Origins"] = new SelectList(_context.InvItemBrands, "Id", "Name");
            ViewData["Brands"]  = new SelectList(_context.InvItemOrigins, "Id", "Name");
            ViewData["StoreAreas"] = new SelectList(storeAreas, "Id", "Name");
            

            return Page();
        }

        public IEnumerable<DialogItems> ConvertItemsToDialogItems(List<InvItem> invItems )
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
                    Name = item.InvCategory.Description + " - " + item.Description,
                    Description = itemspecs + remarkString
                });
            }

            return dialogItems;
        }
    }
}