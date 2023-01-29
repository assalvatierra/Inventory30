using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InvWeb.Data;
using CoreLib.Inventory.Models;
using InvWeb.Data.Services;

namespace InvWeb.Pages.Stores.Adjustment.ItemDetails
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

        public IActionResult OnGet(int hdrId)
        {
            ViewData["InvItemId"] = _itemServices.GetInvItemsSelectList();
            ViewData["InvTrxHdrId"] = new SelectList(_context.InvTrxHdrs, "Id", "Id", hdrId);
            ViewData["InvUomId"] = _itemServices.GetConvertableUomSelectList();
            ViewData["InvTrxDtlOperatorId"] = new SelectList(_context.InvTrxDtlOperators, "Id", "Description");
            ViewData["HdrId"] = hdrId;
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

            _context.InvTrxDtls.Add(InvTrxDtl);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Details", new { id = InvTrxDtl.InvTrxHdrId });
        }
    }
}
