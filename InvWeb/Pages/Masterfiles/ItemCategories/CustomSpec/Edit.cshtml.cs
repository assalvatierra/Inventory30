using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.ItemCategories.CustomSpec
{
    public class EditModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public EditModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InvCatCustomSpec InvCatCustomSpec { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvCatCustomSpec = await _context.InvCatCustomSpecs
                .Include(i => i.InvCategory)
                .Include(i => i.InvCustomSpec).FirstOrDefaultAsync(m => m.Id == id);

            if (InvCatCustomSpec == null)
            {
                return NotFound();
            }
           ViewData["InvCategoryId"] = new SelectList(_context.InvCategories, "Id", "Id");
           ViewData["InvItemCustomSpecTypeId"] = new SelectList(_context.InvCustomSpecs, "Id", "Id");
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

            _context.Attach(InvCatCustomSpec).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvCatCustomSpecExists(InvCatCustomSpec.Id))
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

        private bool InvCatCustomSpecExists(int id)
        {
            return _context.InvCatCustomSpecs.Any(e => e.Id == id);
        }
    }
}
