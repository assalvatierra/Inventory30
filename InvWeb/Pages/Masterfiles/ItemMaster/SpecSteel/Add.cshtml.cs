using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Masterfiles.ItemMaster.SpecSteel
{
    public class AddModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["InvItemId"] = new SelectList(_context.InvItems, "Id", "Description");
            return Page();
        }

        [BindProperty]
        public InvItemSpec_Steel InvItemSpec_Steel { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InvItemSpec_Steel.Add(InvItemSpec_Steel);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Details", new { id = InvItemSpec_Steel.InvItemId });
        }
    }
}
