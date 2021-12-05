using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.ItemWarning
{
    public class DetailsModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public DetailsModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public InvWarningLevel InvWarningLevel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvWarningLevel = await _context.InvWarningLevel
                .Include(i => i.InvItem)
                .Include(i => i.InvUom)
                .Include(i => i.InvWarningType).FirstOrDefaultAsync(m => m.Id == id);

            if (InvWarningLevel == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
