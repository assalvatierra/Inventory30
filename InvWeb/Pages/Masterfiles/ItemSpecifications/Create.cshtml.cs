using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InvWeb.Data;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Masterfiles.ItemSpecifications
{
    public class CreateModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public CreateModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["InvCustomSpecId"] = new SelectList(_context.InvCustomSpecs, "Id", "Id");
        ViewData["InvItemId"] = new SelectList(_context.InvItems, "Id", "Id");
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

            return RedirectToPage("./Index");
        }
    }
}
