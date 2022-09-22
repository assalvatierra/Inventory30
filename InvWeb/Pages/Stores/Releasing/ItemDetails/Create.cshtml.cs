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
using Microsoft.Extensions.Logging;
using InvWeb.Data.Interfaces;

namespace InvWeb.Pages.Stores.Releasing.ItemDetails
{
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel> _logger;
        private readonly InvWeb.Data.ApplicationDbContext _context;
        private readonly ItemServices _itemServices;
        private readonly StoreServices _storeServices;
        private readonly UomServices _uomServices;


        public CreateModel(ILogger<IndexModel> logger, InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
            _itemServices = new ItemServices(context);
            _uomServices = new UomServices(context);
            _storeServices = new StoreServices(context, logger);
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

            var invHdr = _context.InvTrxHdrs.Find(hdrId);

            int storeId = invHdr.InvStoreId;
            int itemId = invItemId == null ? 1 : (int)invItemId;
            var LotNoList = _itemServices.GetLotNotItemList(itemId, storeId);
            var LotNoItemsIds = LotNoList.Select(c => c.LotNo).ToList();
            var selectedItem = " ";

            var storeItems = _storeServices.GetStoreItemsSummary(storeId);
            var availbaleStoreItems = storeItems.Result.Where(i => i.Available > 0).Select(i=>i.Id).ToList();


            ViewData["LotNo"] = new SelectList(LotNoList.Select(x => new {
                Name = String.Format("{0} ", x.LotNo),
                Value = x.LotNo
            }), "Value", "Name");

            ViewData["InvItemId"] = _itemServices.GetInStockedInvItemsSelectList(itemId, availbaleStoreItems);
            ViewData["InvTrxHdrId"] = new SelectList(_context.InvTrxHdrs, "Id", "Id", hdrId);
            ViewData["InvUomId"] = _uomServices.GetUomSelectListByItemId(invItemId);
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
