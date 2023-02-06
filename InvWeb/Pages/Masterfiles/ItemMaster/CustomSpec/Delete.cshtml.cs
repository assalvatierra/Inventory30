using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Masterfiles.ItemMaster.CustomSpec
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvItemCustomSpec = await _context.InvItemCustomSpecs.FindAsync(id);

            if (InvItemCustomSpec != null)
            {
                _context.InvItemCustomSpecs.Remove(InvItemCustomSpec);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("../Details", new { id = InvItemCustomSpec.InvItemId });
        }
    }
}
