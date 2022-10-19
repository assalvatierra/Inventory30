using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.ItemCustomSpec
{
    public class DeleteModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public DeleteModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InvCustomSpec InvCustomSpec { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvCustomSpec = await _context.InvCustomSpecs
                .Include(i => i.InvCustomSpecType).FirstOrDefaultAsync(m => m.Id == id);

            if (InvCustomSpec == null)
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

            InvCustomSpec = await _context.InvCustomSpecs.FindAsync(id);

            if (InvCustomSpec != null)
            {
                _context.InvCustomSpecs.Remove(InvCustomSpec);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
