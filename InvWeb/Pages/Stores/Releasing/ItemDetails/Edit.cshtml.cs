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
using CoreLib.Inventory.Models.Items;
using CoreLib.Inventory.Interfaces;
using CoreLib.Inventory.Interfaces;
using CoreLib.Models.Inventory;
using Microsoft.Extensions.Logging;

namespace InvWeb.Pages.Stores.Releasing.ItemDetails
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;
        private readonly IItemServices _itemServices;
        private readonly IUomServices _uomServices;
        private readonly IStoreServices _storeServices;

        public EditModel(ApplicationDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
            _itemServices = new ItemServices(context);
            _uomServices = new UomServices(context);
            _storeServices = new StoreServices(context, logger);
        }

        [BindProperty]
        public InvTrxDtl InvTrxDtl { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, int? invItemId)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvTrxDtl = await _context.InvTrxDtls
                .Include(i => i.InvItem)
                .ThenInclude(i=>i.InvWarningLevels)
                .ThenInclude(i=>i.InvWarningType)
                .Include(i => i.InvTrxHdr)
                .Include(i => i.InvUom)
                .FirstOrDefaultAsync(m => m.Id == id);


            if (InvTrxDtl == null)
            {
                return NotFound();
            }

            int storeId = InvTrxDtl.InvTrxHdr.InvStoreId;
            int itemId = invItemId  == null ? InvTrxDtl.InvItemId: (int)invItemId;
            var LotNoList = _itemServices.GetLotNotItemList(itemId, storeId);
            var LotNoItemsIds = LotNoList.Select(c => c.LotNo).ToList();
            var selectedItem = " (" + InvTrxDtl.InvItem.Code + ") " + InvTrxDtl.InvItem.Description
                               + " " + InvTrxDtl.InvItem.Remarks;

            var storeItems = _storeServices.GetStoreItemsSummary(storeId, 0, null);
            var availbaleStoreItems = storeItems.Result.Where(i => i.Available > 0).Select(i => i.Id).ToList();

            InvTrxDtl.InvItemId = itemId;


            ViewData["LotNo"] = new SelectList(LotNoList.Select(x => new {
                    Name = String.Format("{0} ", x.LotNo),
                    Value = x.LotNo
                }), "Value", "Name");

            ViewData["InvItemId"] = new SelectList(_itemServices.GetInStockedInvItemsSelectList(itemId, availbaleStoreItems)
                                    .Include(i => i.InvCategory)
                                   .Select(x => new
                                   {
                                       Name = String.Format("{0} - {1} - {2} {3}",
                                       x.Code, x.InvCategory.Description, x.Description, x.Remarks),
                                       Value = x.Id
                                   }), "Value", "Name", invItemId);

            ViewData["InvTrxHdrId"] = new SelectList(_context.InvTrxHdrs, "Id", "Id");
            ViewData["InvUomId"] = new SelectList(_uomServices.GetUomSelectListByItemId(invItemId), "Id", "uom", InvTrxDtl.InvItem.InvUomId);
            ViewData["InvTrxDtlOperatorId"] = new SelectList(_context.InvTrxDtlOperators, "Id", "Description");
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

            _context.Attach(InvTrxDtl).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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
            return _context.InvTrxDtls.Any(e => e.Id == id);
        }


    }
}
