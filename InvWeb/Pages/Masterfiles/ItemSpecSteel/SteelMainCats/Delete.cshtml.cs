using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Masterfiles.ItemSpecSteel.SteelMainCats
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public SteelMainCat SteelMainCat { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.SteelMainCats == null)
            {
                return NotFound();
            }

            var steelmaincat = await _context.SteelMainCats.FirstOrDefaultAsync(m => m.Id == id);

            if (steelmaincat == null)
            {
                return NotFound();
            }
            else 
            {
                SteelMainCat = steelmaincat;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.SteelMainCats == null)
            {
                return NotFound();
            }
            var steelmaincat = await _context.SteelMainCats.FindAsync(id);

            if (steelmaincat != null)
            {
                SteelMainCat = steelmaincat;
                _context.SteelMainCats.Remove(SteelMainCat);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
