using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InvWeb.Pages.Masterfiles.InvItemMasters
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InvItemMaster InvItemMaster { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.InvItemMasters == null)
            {
                return NotFound();
            }

            var invitemmaster =  await _context.InvItemMasters.FirstOrDefaultAsync(m => m.Id == id);
            if (invitemmaster == null)
            {
                return NotFound();
            }

            InvItemMaster = invitemmaster;

           ViewData["InvItemId"] = new SelectList(_context.Set<InvItem>(), "Id", "Description");
           ViewData["InvItemBrandId"] = new SelectList(_context.Set<InvItemBrand>(), "Id", "Name");
           ViewData["InvItemOriginId"] = new SelectList(_context.Set<InvItemOrigin>(), "Id", "Name");
           ViewData["InvUomId"] = new SelectList(_context.Set<InvUom>(), "Id", "uom");
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

            _context.Attach(InvItemMaster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvItemMasterExists(InvItemMaster.Id))
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

        private bool InvItemMasterExists(int id)
        {
          return (_context.InvItemMasters?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
