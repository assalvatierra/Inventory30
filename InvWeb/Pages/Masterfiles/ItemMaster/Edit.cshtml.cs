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

namespace InvWeb.Pages.Masterfiles.ItemMaster
{
    public class EditModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public EditModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InvItem InvItem { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvItem = await _context.InvItems
                .Include(i => i.InvUom).FirstOrDefaultAsync(m => m.Id == id);

            if (InvItem == null)
            {
                return NotFound();
            }
           ViewData["InvUomId"] = new SelectList(_context.Set<InvUom>(), "Id", "Id");
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

            _context.Attach(InvItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvItemExists(InvItem.Id))
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

        private bool InvItemExists(int id)
        {
            return _context.InvItems.Any(e => e.Id == id);
        }
    }
}
