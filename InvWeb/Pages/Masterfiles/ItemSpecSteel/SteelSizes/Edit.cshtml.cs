using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Masterfiles.ItemSpecSteel.SteelSizes
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SteelSize SteelSizes { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.SteelSizes == null)
            {
                return NotFound();
            }

            var steelSizes =  await _context.SteelSizes.FirstOrDefaultAsync(m => m.Id == id);
            if (steelSizes == null)
            {
                return NotFound();
            }
            SteelSizes = steelSizes;
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

            _context.Attach(SteelSizes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SteelOriginExists(SteelSizes.Id))
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

        private bool SteelOriginExists(int id)
        {
          return (_context.SteelSizes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
