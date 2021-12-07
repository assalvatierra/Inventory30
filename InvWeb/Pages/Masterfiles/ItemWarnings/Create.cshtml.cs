using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.ItemWarnings
{
    public class CreateModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public CreateModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int id)
        {
        ViewData["InvItemId"] = new SelectList(_context.InvItems, "Id", "Id", id);
        ViewData["InvUomId"] = new SelectList(_context.InvUoms, "Id", "Id");
        ViewData["InvWarningTypeId"] = new SelectList(_context.Set<InvWarningType>(), "Id", "Id");
            return Page();
        }

        [BindProperty]
        public InvWarningLevel InvWarningLevel { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InvWarningLevels.Add(InvWarningLevel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { id = InvWarningLevel.InvItemId });
        }
    }
}
