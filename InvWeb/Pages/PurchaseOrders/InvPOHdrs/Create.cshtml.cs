using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreLib.Inventory.Models;
using System.Security.Claims;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.PurchaseOrders.InvPOHdrs
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["InvStoreId"] = new SelectList(_context.InvStores, "Id", "StoreName");
            ViewData["InvSupplierId"] = new SelectList(_context.InvSuppliers, "Id", "Name");
            ViewData["InvPoHdrStatusId"] = new SelectList(_context.InvPoHdrStatus, "Id", "Status");
            ViewData["UserId"] = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Page();
        }

        [BindProperty]
        public InvPoHdr InvPoHdr { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InvPoHdrs.Add(InvPoHdr);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
