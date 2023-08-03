using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Masterfiles.ItemSpecSteel
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["InvItemId"] = new SelectList(_context.InvItems, "Id", "Id");
        ViewData["SteelBrandId"] = new SelectList(_context.Set<SteelBrand>(), "Id", "Id");
        ViewData["SteelMainCatId"] = new SelectList(_context.Set<SteelMainCat>(), "Id", "Id");
        ViewData["SteelMaterialId"] = new SelectList(_context.Set<SteelMaterial>(), "Id", "Id");
        ViewData["SteelMaterialGradeId"] = new SelectList(_context.Set<SteelMaterialGrade>(), "Id", "Id");
        ViewData["SteelOriginId"] = new SelectList(_context.Set<SteelOrigin>(), "Id", "Id");
        ViewData["SteelSubCatId"] = new SelectList(_context.Set<SteelSubCat>(), "Id", "Id");
            return Page();
        }

        [BindProperty]
        public InvItemSpec_Steel InvItemSpec_Steel { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.InvItemSpec_Steel == null || InvItemSpec_Steel == null)
            {
                return Page();
            }

            _context.InvItemSpec_Steel.Add(InvItemSpec_Steel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
