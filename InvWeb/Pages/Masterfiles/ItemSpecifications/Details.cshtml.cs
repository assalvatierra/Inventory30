using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.ItemSpecifications
{
    public class DetailsModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public DetailsModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
