using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Masterfiles.ItemSpecSteel
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public InvItemSpec_Steel InvItemSpec_Steel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.InvItemSpec_Steel == null)
            {
                return NotFound();
            }

            var invitemspec_steel = await _context.InvItemSpec_Steel.FirstOrDefaultAsync(m => m.Id == id);

            if (invitemspec_steel == null)
            {
                return NotFound();
            }
            else 
            {
                InvItemSpec_Steel = invitemspec_steel;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.InvItemSpec_Steel == null)
            {
                return NotFound();
            }
            var invitemspec_steel = await _context.InvItemSpec_Steel.FindAsync(id);

            if (invitemspec_steel != null)
            {
                InvItemSpec_Steel = invitemspec_steel;
                _context.InvItemSpec_Steel.Remove(InvItemSpec_Steel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
