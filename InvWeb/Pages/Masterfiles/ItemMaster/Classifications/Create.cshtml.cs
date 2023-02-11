using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Masterfiles.ItemMaster.Classifications
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["InvClassificationId"] = new SelectList(_context.InvClassifications, "Id", "Classification");
            ViewData["InvItemId"] = new SelectList(_context.InvItems, "Id", "Description", id);
            ViewData["ItemId"] = id;
            return Page();
        }

        [BindProperty]
        public InvItemClass InvItemClass { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InvItemClasses.Add(InvItemClass);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Details", new { id = InvItemClass.InvItemId });
        }
    }
}
