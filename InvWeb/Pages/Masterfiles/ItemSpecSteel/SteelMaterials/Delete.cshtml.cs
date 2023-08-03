using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Masterfiles.ItemSpecSteel.SteelMaterials
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public SteelMaterial SteelMaterial { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.SteelMaterials == null)
            {
                return NotFound();
            }

            var steelmaterial = await _context.SteelMaterials.FirstOrDefaultAsync(m => m.Id == id);

            if (steelmaterial == null)
            {
                return NotFound();
            }
            else 
            {
                SteelMaterial = steelmaterial;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.SteelMaterials == null)
            {
                return NotFound();
            }
            var steelmaterial = await _context.SteelMaterials.FindAsync(id);

            if (steelmaterial != null)
            {
                SteelMaterial = steelmaterial;
                _context.SteelMaterials.Remove(SteelMaterial);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
