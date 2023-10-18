using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Masterfiles.ItemMaster
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
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
                .Include(i => i.InvUom)
                .Include(i => i.InvItemSpec_Steel)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (InvItem == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvItem = await _context.InvItems.FindAsync(id);

            DeleteSteelSpecs(InvItem);

            if (InvItem != null)
            {
                _context.InvItems.Remove(InvItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        private bool DeleteSteelSpecs(InvItem invItem)
        {
            if (invItem.InvItemSpec_Steel != null)
            {
                var steelspec = _context.InvItemSpec_Steel.Where(c => c.InvItemId == invItem.Id).FirstOrDefault();

                _context.InvItemSpec_Steel.Remove(steelspec);

                return true;
            }
            return false;
        }
    }
}
