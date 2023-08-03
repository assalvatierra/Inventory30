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

namespace InvWeb.Pages.Masterfiles.ItemSpecSteel
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InvItemSpec_Steel InvItemSpec_Steel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.InvItemSpec_Steel == null)
            {
                return NotFound();
            }

            var invitemspec_steel =  await _context.InvItemSpec_Steel.FirstOrDefaultAsync(m => m.Id == id);
            if (invitemspec_steel == null)
            {
                return NotFound();
            }
            InvItemSpec_Steel = invitemspec_steel;
           ViewData["InvItemId"] = new SelectList(_context.InvItems, "Id", "Name");
           ViewData["SteelBrandId"] = new SelectList(_context.Set<SteelBrand>(), "Id", "Name");
           ViewData["SteelMainCatId"] = new SelectList(_context.Set<SteelMainCat>(), "Id", "Name");
           ViewData["SteelMaterialId"] = new SelectList(_context.Set<SteelMaterial>(), "Id", "Name");
           ViewData["SteelMaterialGradeId"] = new SelectList(_context.Set<SteelMaterialGrade>(), "Id", "Name");
           ViewData["SteelOriginId"] = new SelectList(_context.Set<SteelOrigin>(), "Id", "Name");
           ViewData["SteelSubCatId"] = new SelectList(_context.Set<SteelSubCat>(), "Id", "Name");
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

            _context.Attach(InvItemSpec_Steel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvItemSpec_SteelExists(InvItemSpec_Steel.Id))
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

        private bool InvItemSpec_SteelExists(int id)
        {
          return (_context.InvItemSpec_Steel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
