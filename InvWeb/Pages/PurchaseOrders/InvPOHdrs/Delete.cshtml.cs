using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.PurchaseOrders.InvPOHdrs
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InvPoHdr InvPoHdr { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvPoHdr = await _context.InvPoHdrs
                .Include(i => i.InvStore)
                .Include(i => i.InvSupplier).FirstOrDefaultAsync(m => m.Id == id);

            if (InvPoHdr == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvPoHdr = await _context.InvPoHdrs.FindAsync(id);

            if (InvPoHdr != null)
            {
                _context.InvPoHdrs.Remove(InvPoHdr);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
