using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Masterfiles.ItemSpecSteel.SteelBrands
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SteelBrand SteelBrand { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.SteelBrands == null)
            {
                return NotFound();
            }

            var steelbrand =  await _context.SteelBrands.FirstOrDefaultAsync(m => m.Id == id);
            if (steelbrand == null)
            {
                return NotFound();
            }
            SteelBrand = steelbrand;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(SteelBrand).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SteelBrandExists(SteelBrand.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SteelBrandExists(int id)
        {
          return (_context.SteelBrands?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
