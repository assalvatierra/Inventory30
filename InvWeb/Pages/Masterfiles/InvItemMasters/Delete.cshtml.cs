using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

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
        public InvItemMaster InvItemMaster { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvItemMaster = await _context.InvItemMasters
                .Include(i => i.InvItem)
                .Include(i => i.InvUom)
                .Include(i => i.InvItemBrand)
                .Include(i => i.InvItemOrigin)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (InvItemMaster == null)
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

            InvItemMaster = await _context.InvItemMasters.FindAsync(id);

            if (InvItemMaster != null)
            {
                _context.InvItemMasters.Remove(InvItemMaster);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
