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
using InvWeb.Data.Interfaces;

namespace InvWeb.Pages.Stores.Releasing.ItemDetails
{
    public class EditModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;
        private readonly ItemServices _itemServices;
        private readonly UomServices _uomServices;

        public EditModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
            _itemServices = new ItemServices(context);
            _uomServices = new UomServices(context);
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

            InvTrxDtl.InvItemId = itemId;


            ViewData["LotNo"] = new SelectList(LotNoList.Select(x => new {
                    Name = String.Format("{0} ", x.LotNo),
                    Value = x.LotNo
                }), "Value", "Name");

            ViewData["InvItemId"] = _itemServices.GetInvItemsSelectList(itemId);
            ViewData["InvTrxHdrId"] = new SelectList(_context.InvTrxHdrs, "Id", "Id");
            ViewData["InvUomId"] = new SelectList(_uomServices.GetUomSelectListByItemId(InvTrxDtl.InvItemId), "Id", "uom", InvTrxDtl.InvItem.InvUomId);
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
