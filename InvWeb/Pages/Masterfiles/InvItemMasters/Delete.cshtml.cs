using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLib.Models.Inventory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InvWeb.Pages.Masterfiles.InvItemMasters
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
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

            var invitemmaster = await _context.InvItemMasters.FirstOrDefaultAsync(m => m.Id == id);

            if (invitemmaster == null)
            {
                return NotFound();
            }
            else 
            {
                InvItemMaster = invitemmaster;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.InvItemMasters == null)
            {
                return NotFound();
            }
            var invitemmaster = await _context.InvItemMasters.FindAsync(id);

            if (invitemmaster != null)
            {
                InvItemMaster = invitemmaster;
                _context.InvItemMasters.Remove(InvItemMaster);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
