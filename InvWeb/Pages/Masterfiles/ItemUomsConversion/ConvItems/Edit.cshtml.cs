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

namespace InvWeb.Pages.Masterfiles.ItemUomsConversion.ConvItems
{
    public class EditModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public EditModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InvUomConvItem InvUomConvItem { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvUomConvItem = await _context.InvUomConvItem
                .Include(i => i.InvClassification)
                .Include(i => i.InvItem)
                .Include(i => i.InvUomConversion).FirstOrDefaultAsync(m => m.Id == id);

            if (InvUomConvItem == null)
            {
                return NotFound();
            }
           ViewData["InvClassificationId"] = new SelectList(_context.InvClassifications, "Id", "Id");
           ViewData["InvItemId"] = new SelectList(_context.InvItems, "Id", "Id");
           ViewData["InvUomConversionId"] = new SelectList(_context.InvUomConversions, "Id", "Id");
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

            _context.Attach(InvUomConvItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvUomConvItemExists(InvUomConvItem.Id))
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

        private bool InvUomConvItemExists(int id)
        {
            return _context.InvUomConvItem.Any(e => e.Id == id);
        }
    }
}
