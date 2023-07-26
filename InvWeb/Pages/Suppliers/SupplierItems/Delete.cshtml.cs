using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Suppliers.SupplierItems
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InvSupplierItem InvSupplierItem { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvSupplierItem = await _context.InvSupplierItems
                .Include(i => i.InvItem)
                .Include(i => i.InvSupplier).FirstOrDefaultAsync(m => m.Id == id);

            if (InvSupplierItem == null)
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

            InvSupplierItem = await _context.InvSupplierItems.FindAsync(id);

            if (InvSupplierItem != null)
            {
                _context.InvSupplierItems.Remove(InvSupplierItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new { id = InvSupplierItem.InvSupplierId });
        }
    }
}
