using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Masterfiles.ItemSpecifications
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InvItemCustomSpec InvItemCustomSpec { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvItemCustomSpec = await _context.InvItemCustomSpecs
                .Include(i => i.InvCustomSpec)
                .Include(i => i.InvItem).FirstOrDefaultAsync(m => m.Id == id);

            if (InvItemCustomSpec == null)
            {
                return NotFound();
            }
           ViewData["InvCustomSpecId"] = new SelectList(_context.InvCustomSpecs, "Id", "Id");
           ViewData["InvItemId"] = new SelectList(_context.InvItems, "Id", "Id");
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

            _context.Attach(InvItemCustomSpec).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvItemCustomSpecExists(InvItemCustomSpec.Id))
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

        private bool InvItemCustomSpecExists(int id)
        {
            return _context.InvItemCustomSpecs.Any(e => e.Id == id);
        }
    }
}
