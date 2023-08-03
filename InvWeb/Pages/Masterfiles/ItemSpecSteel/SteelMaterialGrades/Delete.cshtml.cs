using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Masterfiles.ItemSpecSteel.SteelMaterialGrades
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public SteelMaterialGrade SteelMaterialGrade { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.SteelMaterialGrades == null)
            {
                return NotFound();
            }

            var steelmaterialgrade = await _context.SteelMaterialGrades.FirstOrDefaultAsync(m => m.Id == id);

            if (steelmaterialgrade == null)
            {
                return NotFound();
            }
            else 
            {
                SteelMaterialGrade = steelmaterialgrade;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.SteelMaterialGrades == null)
            {
                return NotFound();
            }
            var steelmaterialgrade = await _context.SteelMaterialGrades.FindAsync(id);

            if (steelmaterialgrade != null)
            {
                SteelMaterialGrade = steelmaterialgrade;
                _context.SteelMaterialGrades.Remove(SteelMaterialGrade);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
