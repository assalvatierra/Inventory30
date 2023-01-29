using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Masterfiles.ItemCustomSpecType
{
    public class DeleteModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public DeleteModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InvCustomSpecType InvCustomSpecType { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvCustomSpecType = await _context.InvCustomSpecTypes.FirstOrDefaultAsync(m => m.Id == id);

            if (InvCustomSpecType == null)
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

            InvCustomSpecType = await _context.InvCustomSpecTypes.FindAsync(id);

            if (InvCustomSpecType != null)
            {
                _context.InvCustomSpecTypes.Remove(InvCustomSpecType);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
