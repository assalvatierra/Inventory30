using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InvWeb.Data;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Masterfiles.ItemCategories.CustomSpec
{
    public class AddModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public AddModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("../Index");
            }

            ViewData["InvCategoryId"] = new SelectList(_context.InvCategories, "Id", "Id", id);
            ViewData["InvItemCustomSpecTypeId"] = new SelectList(_context.InvCustomSpecs, "Id", "SpecName");
            return Page();
        }

        [BindProperty]
        public InvCatCustomSpec InvCatCustomSpec { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InvCatCustomSpecs.Add(InvCatCustomSpec);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Index");
        }
    }
}
