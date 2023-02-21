using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InvWeb.Data.Services;
using CoreLib.Inventory.Models;
using CoreLib.Inventory.Models.Items;
using Microsoft.Extensions.Logging;
using CoreLib.Inventory.Interfaces;
using CoreLib.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using Modules.Inventory;

namespace InvWeb.Pages.Stores.Releasing.ItemDetails
{
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IItemServices _itemServices;
        private readonly IStoreServices _storeServices;
        private readonly IUomServices _uomServices;
        private readonly IItemDtlsServices _itemDtlsServices;
        private readonly IItemTrxServices _itemTrxServices;



        public CreateModel(ILogger<CreateModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            _itemServices = new ItemServices(context);
            _uomServices = new UomServices(context);
            _storeServices = new StoreServices(context, _logger);
            _itemDtlsServices = new ItemDtlsServices(context, _logger);
            _itemTrxServices = new ItemTrxServices(context, _logger);
        }

        public IActionResult OnGet(int? hdrId, int? invItemId)
        {
            if (hdrId == null)
            {
                return NotFound();
            }

            if (invItemId == null)
            {
                invItemId = 2;
            }

            int storeId = _itemTrxServices.GetInvTrxStoreId((int)hdrId);
            int itemId = GetDefaultInvitemId(invItemId);
            var LotNoList = _itemServices.GetLotNotItemList(itemId, storeId);
            var availableItems = _storeServices.GetAvailableItemsIdsByStore(storeId);

            ViewData["LotNo"] = new SelectList(LotNoList.Select(x => new {
                Name = String.Format("{0} ", x.LotNo),
                Value = x.LotNo
            }), "Value", "Name");

            ViewData["InvItemId"] = new SelectList(_itemServices.GetInStockedInvItemsSelectList(itemId, availableItems)
                                    .Include(i => i.InvCategory)
                                    .Select(x => new {
                                       Name = String.Format("{0} - {1} - {2} {3}",
                                       x.Code, x.InvCategory.Description, x.Description, x.Remarks),
                                       Value = x.Id
                                     }), "Value", "Name", itemId);
            
            ViewData["InvUomId"] = new SelectList(_uomServices.GetUomSelectListByItemId(invItemId), "Id", "uom");
            ViewData["InvTrxHdrId"] = new SelectList(_itemTrxServices.GetInvTrxHdrs(), "Id", "Id", hdrId);
            ViewData["InvTrxDtlOperatorId"] = new SelectList(_itemDtlsServices.GetInvTrxDtlOperators(), "Id", "Description", 2);
            ViewData["HdrId"] = hdrId;
            ViewData["LotNoItems"] = LotNoList;
            ViewData["StoreId"] = storeId;
            ViewData["SelectedItem"] = " ";

            return Page();
        }

        [BindProperty]
        public InvTrxDtl InvTrxDtl { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return Page();
                }

                if (InvTrxDtl.LotNo == 0 || InvTrxDtl.LotNo == null)
                {
                    //requires LotNo
                    return RedirectToPage("../Details", new { id = InvTrxDtl.InvTrxHdrId });
                }

                _itemDtlsServices.CreateInvDtls(InvTrxDtl);
                await _itemDtlsServices.SaveChangesAsync();

                return RedirectToPage("../Details", new { id = InvTrxDtl.InvTrxHdrId });

            }
            catch (Exception ex)
            {

                _logger.LogError("Stores/Releasing/Create: Unable to OnPostAsync " + ex.Message);
                return RedirectToPage("../Details", new { id = InvTrxDtl.InvTrxHdrId });
            }
        }

        private int GetDefaultInvitemId(int? invItemId)
        {
            return invItemId == null ? 1 : (int)invItemId;
        }
    }
}
