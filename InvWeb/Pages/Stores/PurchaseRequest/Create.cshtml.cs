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

namespace InvWeb.Pages.Stores.PurchaseRequest
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int? storeId)
        {
            if (storeId == null)
            {
                return NotFound();
            }

            ViewData["InvPoHdrStatusId"] = new SelectList(_context.InvPoHdrStatus, "Id", "Status");
            ViewData["InvStoreId"] = new SelectList(_context.InvStores, "Id", "StoreName", storeId);
            ViewData["InvSupplierId"] = new SelectList(_context.InvSuppliers, "Id", "Name");
            ViewData["UserId"] = User.FindFirstValue(ClaimTypes.Name);
            ViewData["StoreId"] = storeId ?? 0;
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

            return RedirectToPage("./Index", new { storeId = InvPoHdr.InvStoreId });
        }
    }
}
