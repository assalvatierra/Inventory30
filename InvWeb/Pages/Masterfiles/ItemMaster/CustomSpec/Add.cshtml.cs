using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.ItemMaster.CustomSpec
{
    public class AddModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public AddModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int id)
        {
            ViewData["InvCustomSpecId"] = new SelectList(_context.InvCustomSpecs, "Id", "SpecName");
            ViewData["InvItemId"] = new SelectList(_context.InvItems, "Id", "Description", id);
            ViewData["ItemId"] = id;
            return Page();
        }

        [BindProperty]
        public InvItemCustomSpec InvItemCustomSpec { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InvItemCustomSpecs.Add(InvItemCustomSpec);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Details", new { id = InvItemCustomSpec.InvItemId });
        }
    }
}
