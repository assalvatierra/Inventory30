using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Masterfiles.ItemUomsConversion
{
    public class EditModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public EditModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InvUomConversion InvUomConversion { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvUomConversion = await _context.InvUomConversions.FirstOrDefaultAsync(m => m.Id == id);

            if (InvUomConversion == null)
            {
                return NotFound();
            }

            ViewData["InvUomId_base"] = new SelectList(_context.InvUoms, "Id", "uom");
            ViewData["InvUomId_into"] = new SelectList(_context.InvUoms, "Id", "uom");

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

            _context.Attach(InvUomConversion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvUomConversionExists(InvUomConversion.Id))
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

        private bool InvUomConversionExists(int id)
        {
            return _context.InvUomConversions.Any(e => e.Id == id);
        }
    }
}
