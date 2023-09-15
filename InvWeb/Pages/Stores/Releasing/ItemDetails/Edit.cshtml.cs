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
using CoreLib.DTO.Releasing;

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
        public ReleasingItemDtlsCreateEditModel ItemDtlsCreateEditModel { get; set; }

        //[BindProperty]
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
            var lotNoList = _itemServices.GetLotNotItemList(itemId, storeId);
            var availableItems = _storeServices.GetAvailableItemsIdsByStore(storeId);

            InvTrxDtl.InvItemId = itemId;

            ItemDtlsCreateEditModel = new ReleasingItemDtlsCreateEditModel();
            ItemDtlsCreateEditModel.InvTrxDtl = InvTrxDtl;

            ItemDtlsCreateEditModel.LotNo = new SelectList(lotNoList.Select(x => new {
                Name = String.Format("{0} ", x.LotNo),
                Value = x.LotNo
            }), "Value", "Name", InvTrxDtl.LotNo);

            ItemDtlsCreateEditModel.InvItems = new SelectList(_itemServices.GetInStockedInvItemsSelectList(itemId, availableItems)
                                    .Include(i => i.InvCategory)
                                    .Select(x => new {
                                        Name = String.Format("{0} - {1} - {2} {3}",
                                       x.Code, x.InvCategory.Description, x.Description, x.Remarks),
                                        Value = x.Id
                                    }), "Value", "Name", itemId);

            ItemDtlsCreateEditModel.InvUoms = new SelectList(_uomServices.GetUomSelectListByItemId((int)invItemId), "Id", "uom", InvTrxDtl.InvUomId);
            ItemDtlsCreateEditModel.InvTrxHdrs = new SelectList(_itemTrxServices.GetInvTrxHdrs(), "Id", "Id", InvTrxDtl.InvTrxHdrId);
            ItemDtlsCreateEditModel.InvTrxDtlOperators = new SelectList(_itemDtlsServices.GetInvTrxDtlOperators(), "Id", "Description", 2);
            ItemDtlsCreateEditModel.HrdId = (int)InvTrxDtl.InvTrxHdrId;
            ItemDtlsCreateEditModel.LotNoItems = lotNoList;
            ItemDtlsCreateEditModel.StoreId = storeId;
            ItemDtlsCreateEditModel.SelectedItem = " (" + InvTrxDtl.InvItem.Code + ") " + InvTrxDtl.InvItem.Description
                               + " " + InvTrxDtl.InvItem.Remarks;

            ViewData["SelectedItemId"] = itemId;

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

            if (ItemDtlsCreateEditModel.InvTrxDtl.LotNo == 0 || ItemDtlsCreateEditModel.InvTrxDtl.LotNo == null)
            {
                return RedirectToPage("../Details", new { id = ItemDtlsCreateEditModel.InvTrxDtl.InvTrxHdrId });
            }

            _itemDtlsServices.EditInvDtls(ItemDtlsCreateEditModel.InvTrxDtl);

            try
            {
                await _itemDtlsServices.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvTrxDtlExists(ItemDtlsCreateEditModel.InvTrxDtl.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../Details", new { id = ItemDtlsCreateEditModel.InvTrxDtl.InvTrxHdrId });
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
