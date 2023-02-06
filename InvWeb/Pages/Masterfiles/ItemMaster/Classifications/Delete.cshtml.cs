using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Masterfiles.ItemMaster.Classifications
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InvItemClass InvItemClass { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvItemClass = await _context.InvItemClasses
                .Include(i => i.InvClassification)
                .Include(i => i.InvItem).FirstOrDefaultAsync(m => m.Id == id);

            if (InvItemClass == null)
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

            InvItemClass = await _context.InvItemClasses.FindAsync(id);

            if (InvItemClass != null)
            {
                _context.InvItemClasses.Remove(InvItemClass);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("../Details", new { id = InvItemClass.InvItemId });
        }
    }
}
