using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.ItemClassification
{
    public class DeleteModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public DeleteModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InvClassification InvClassification { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvClassification = await _context.InvClassifications.FirstOrDefaultAsync(m => m.Id == id);

            if (InvClassification == null)
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

            InvClassification = await _context.InvClassifications.FindAsync(id);

            if (InvClassification != null)
            {
                _context.InvClassifications.Remove(InvClassification);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
