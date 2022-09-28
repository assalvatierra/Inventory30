using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.ItemMaster
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
            ViewData["InvCategoryId"] = new SelectList(_context.Set<InvCategory>(), "Id", "Description");
            ViewData["InvUomId"] = new SelectList(_context.Set<InvUom>(), "Id", "uom");
            return Page();
        }

        [BindProperty]
        public InvItem InvItem { get; set; }

        [BindProperty]
        public InvItemSpec_Steel InvItemSpec_Steel { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InvItems.Add(InvItem);
            await _context.SaveChangesAsync();

            await AddItemSpecification();

            return RedirectToPage("./Index");
        }

        public async Task<int> AddItemSpecification()
        {
            if (!ModelState.IsValid)
            {
                return 0;
            }

            if (InvItem.Id == 0 || InvItem == null)
            {
                return 0;
            }

            InvItemSpec_Steel.InvItemId = InvItem.Id;

            _context.InvItemSpec_Steel.Add(InvItemSpec_Steel);
            return await _context.SaveChangesAsync();
        }
    }
}
