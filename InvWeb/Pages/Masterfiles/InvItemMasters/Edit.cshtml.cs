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
        public InvItemMaster InvItemMaster { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvItemMaster = await _context.InvItemMasters.FirstOrDefaultAsync(m => m.Id == id);

            if (InvItemMaster == null)
            {
                return NotFound();
            }

            ViewData["InvItemId"] = new SelectList(_context.Set<InvItem>(), "Id", "Description", InvItemMaster.InvItemId);
            ViewData["InvItemBrandId"] = new SelectList(_context.Set<InvItemBrand>(), "Id", "Name", InvItemMaster.InvItemBrandId);
            ViewData["InvItemOriginId"] = new SelectList(_context.Set<InvItemOrigin>(), "Id", "Name", InvItemMaster.InvItemOriginId);
            ViewData["InvUomId"] = new SelectList(_context.Set<InvUom>(), "Id", "uom", InvItemMaster.InvUomId);

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
                if (!InvClassificationExists(InvItemMaster.Id))
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

        private bool InvClassificationExists(int id)
        {
            return _context.InvItemMasters.Any(e => e.Id == id);
        }
    }
}
