using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Masterfiles.ItemSpecSteel.SteelSubCats
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public SteelSubCat SteelSubCat { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.SteelSubCats == null)
            {
                return NotFound();
            }

            var steelsubcat = await _context.SteelSubCats.FirstOrDefaultAsync(m => m.Id == id);

            if (steelsubcat == null)
            {
                return NotFound();
            }
            else 
            {
                SteelSubCat = steelsubcat;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.SteelSubCats == null)
            {
                return NotFound();
            }
            var steelsubcat = await _context.SteelSubCats.FindAsync(id);

            if (steelsubcat != null)
            {
                SteelSubCat = steelsubcat;
                _context.SteelSubCats.Remove(SteelSubCat);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
