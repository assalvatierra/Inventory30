using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InvWeb.Data.Services;
using InvWeb.Data;
using WebDBSchema.Models;
using WebDBSchema.Models.Items;

namespace InvWeb.Pages.Stores.Releasing.ItemDetails
{
    public class CreateModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;
        private readonly ItemServices _itemServices;


        public CreateModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
            _itemServices = new ItemServices(context);
        }

        public IActionResult OnGet(int? hdrId, int? invItemId)
        {
            if (hdrId == null)
            {
                return NotFound();
            }

            var invHdr = _context.InvTrxHdrs.Find(hdrId);

            int storeId = invHdr.InvStoreId;
            int itemId = invItemId == null ? 1 : (int)invItemId;
            var LotNoList = _itemServices.GetLotNotItemList(itemId, storeId);
            var LotNoItemsIds = LotNoList.Select(c => c.LotNo).ToList();
            var selectedItem = " ";

            //InvTrxDtl.InvItemId = itemId;


            ViewData["LotNo"] = new SelectList(LotNoList.Select(x => new {
                Name = String.Format("{0} ", x.LotNo),
                Value = x.LotNo
            }), "Value", "Name");

            ViewData["InvItemId"] = _itemServices.GetInvItemsSelectList(itemId);
            ViewData["InvTrxHdrId"] = new SelectList(_context.InvTrxHdrs, "Id", "Id", hdrId);
            ViewData["InvUomId"] = new SelectList(_context.InvUoms, "Id", "uom");
            ViewData["InvTrxDtlOperatorId"] = new SelectList(_context.InvTrxDtlOperators, "Id", "Description", 2);
            ViewData["HdrId"] = hdrId;
            ViewData["LotNoItems"] = LotNoList;
            ViewData["StoreId"] = storeId;
            ViewData["SelectedItem"] = selectedItem;

            return Page();
        }

        [BindProperty]
        public InvTrxDtl InvTrxDtl { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
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

            _context.InvTrxDtls.Add(InvTrxDtl);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Details", new { id = InvTrxDtl.InvTrxHdrId });
        }
    }
}
