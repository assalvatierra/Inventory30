using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data.Services;
using CoreLib.Inventory.Models;
using CoreLib.Inventory.Interfaces;
using CoreLib.Models.Inventory;
using Microsoft.Extensions.Logging;
using Modules.Inventory;

namespace InvWeb.Pages.Stores.Releasing.ItemDetails
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;
        private readonly IItemServices _itemServices;
        private readonly IUomServices _uomServices;
        private readonly IStoreServices _storeServices;
        private readonly IItemDtlsServices _itemDtlsServices;
        private readonly IItemTrxServices _itemTrxServices;

        public EditModel(ApplicationDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
            _itemServices = new ItemServices(context);
            _uomServices = new UomServices(context);
            _storeServices = new StoreServices(context, logger);
            _itemDtlsServices = new ItemDtlsServices(context, _logger);
            _itemTrxServices = new ItemTrxServices(context, _logger);
        }

        [BindProperty]
        public InvTrxDtl InvTrxDtl { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, int? invItemId)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvTrxDtl = await _itemDtlsServices.GetInvDtlsByIdOnEdit((int)id);

            if (InvTrxDtl == null)
            {
                return NotFound();
            }

            int storeId = InvTrxDtl.InvTrxHdr.InvStoreId;
            int itemId = GetDefaultInvitemId(invItemId);
            var LotNoList = _itemServices.GetLotNotItemList(itemId, storeId);
            var selectedItem = " (" + InvTrxDtl.InvItem.Code + ") " + InvTrxDtl.InvItem.Description
                               + " " + InvTrxDtl.InvItem.Remarks;

            var availableItems = _storeServices.GetAvailableItemsIdsByStore(storeId);

            InvTrxDtl.InvItemId = itemId;


            ViewData["LotNo"] = new SelectList(LotNoList.Select(x => new {
                    Name = String.Format("{0} ", x.LotNo),
                    Value = x.LotNo
                }), "Value", "Name");

            ViewData["InvItemId"] = new SelectList(_itemServices.GetInStockedInvItemsSelectList(itemId, availableItems)
                                    .Include(i => i.InvCategory)
                                   .Select(x => new
                                   {
                                       Name = String.Format("{0} - {1} - {2} {3}",
                                       x.Code, x.InvCategory.Description, x.Description, x.Remarks),
                                       Value = x.Id
                                   }), "Value", "Name", invItemId);

            ViewData["InvTrxHdrId"] = new SelectList(_itemTrxServices.GetInvTrxHdrs(), "Id", "Id");
            ViewData["InvUomId"] = new SelectList(_uomServices.GetUomSelectListByItemId(invItemId), "Id", "uom", InvTrxDtl.InvItem.InvUomId);
            ViewData["InvTrxDtlOperatorId"] = new SelectList(_itemDtlsServices.GetInvTrxDtlOperators(), "Id", "Description");
            ViewData["LotNoItems"]  = LotNoList;
            ViewData["StoreId"]     = storeId;
            ViewData["SelectedItem"] = selectedItem;

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (InvTrxDtl.LotNo == 0 || InvTrxDtl.LotNo == null)
            {
                return RedirectToPage("../Details", new { id = InvTrxDtl.InvTrxHdrId });
            }

            _itemDtlsServices.EditInvDtls(InvTrxDtl);

            try
            {
                await _itemDtlsServices.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvTrxDtlExists(InvTrxDtl.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../Details", new { id = InvTrxDtl.InvTrxHdrId });
        }

        private bool InvTrxDtlExists(int id)
        {
            return _itemDtlsServices.InvTrxDtlsExists(id);
        }

        private int GetDefaultInvitemId(int? invItemId)
        {
            return invItemId == null ? InvTrxDtl.InvItemId : (int)invItemId; ;
        }
    }
}
