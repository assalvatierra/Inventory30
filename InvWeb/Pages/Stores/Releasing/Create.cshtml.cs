using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InvWeb.Data;
using WebDBSchema.Models;
using System.Security.Claims;

namespace InvWeb.Pages.Stores.Releasing
{
    public class CreateModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public CreateModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int? storeId)
        {
            if (storeId == null)
            {
                return NotFound();
            }

            ViewData["InvStoreId"] = new SelectList(_context.InvStores, "Id", "StoreName", storeId);
            ViewData["InvTrxHdrStatusId"] = new SelectList(_context.InvTrxHdrStatus, "Id", "Status");
            ViewData["InvTrxTypeId"] = new SelectList(_context.invTrxTypes, "Id", "Type", 2);
            ViewData["UserId"] = User.FindFirstValue(ClaimTypes.Name);
            ViewData["StoreId"] = storeId;

            return Page();
        }

        [BindProperty]
        public InvTrxHdr InvTrxHdr { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InvTrxHdrs.Add(InvTrxHdr);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
