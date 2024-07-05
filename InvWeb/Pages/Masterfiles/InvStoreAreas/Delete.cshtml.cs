using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Masterfiles.ItemStoreArea
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InvStoreArea InvStoreArea { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvStoreArea = await _context.InvStoreAreas.FirstOrDefaultAsync(m => m.Id == id);

            if (InvStoreArea == null)
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

            InvStoreArea = await _context.InvStoreAreas.FindAsync(id);

            if (InvStoreArea != null)
            {
                _context.InvStoreAreas.Remove(InvStoreArea);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
