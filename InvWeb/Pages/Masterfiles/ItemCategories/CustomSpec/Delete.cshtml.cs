using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Masterfiles.ItemCategories.CustomSpec
{
    public class DeleteModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public DeleteModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InvCatCustomSpec InvCatCustomSpec { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvCatCustomSpec = await _context.InvCatCustomSpecs
                .Include(i => i.InvCategory)
                .Include(i => i.InvCustomSpec).FirstOrDefaultAsync(m => m.Id == id);

            if (InvCatCustomSpec == null)
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

            InvCatCustomSpec = await _context.InvCatCustomSpecs.FindAsync(id);

            if (InvCatCustomSpec != null)
            {
                _context.InvCatCustomSpecs.Remove(InvCatCustomSpec);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("../Index");
        }
    }
}
