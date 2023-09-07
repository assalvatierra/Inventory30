using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Masterfiles.ItemSpecSteel.SteelSizes
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public SteelSize SteelSizes { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.SteelSizes == null)
            {
                return NotFound();
            }

            var steelSizes = await _context.SteelSizes.FirstOrDefaultAsync(m => m.Id == id);

            if (steelSizes == null)
            {
                return NotFound();
            }
            else 
            {
                SteelSizes = steelSizes;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.SteelSizes == null)
            {
                return NotFound();
            }
            var steelSizes = await _context.SteelSizes.FindAsync(id);

            if (steelSizes != null)
            {
                SteelSizes = steelSizes;
                _context.SteelSizes.Remove(SteelSizes);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
