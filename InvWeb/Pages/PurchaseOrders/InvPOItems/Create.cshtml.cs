using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.PurchaseOrders.InvPOItems
{
    public class CreateModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public CreateModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int? invPoHdrid)
        {
            if (invPoHdrid == null)
            {
                RedirectToAction("../InvPOHdrs/Index");
            }

            ViewData["InvItemId"] = new SelectList(_context.InvItems, "Id", "Description");
            ViewData["InvPoHdrId"] = new SelectList(_context.InvPoHdrs, "Id", "Id", invPoHdrid);
            ViewData["InvUomId"] = new SelectList(_context.InvUoms, "Id", "uom");
            ViewData["PoHdrid"] = invPoHdrid;

            return Page();
        }

        [BindProperty]
        public InvPoItem InvPoItem { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InvPoItems.Add(InvPoItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("../InvPOHdrs/Details", new { id = InvPoItem.InvPoHdrId });
        }
    }
}
