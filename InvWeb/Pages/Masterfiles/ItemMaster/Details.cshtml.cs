using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.ItemMaster
{
    public class DetailsModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public DetailsModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public InvItem InvItem { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvItem = await _context.InvItems
                .Include(i => i.InvUom)
                .Include(i => i.InvItemClasses)
                .Include(i => i.InvWarningLevels)
                    .ThenInclude(i => i.InvWarningType)
                .Include(i=> i.InvItemSpec_Steel)
                .FirstOrDefaultAsync(m => m.Id == id);


            if (InvItem == null)
            {
                return NotFound();
            }

            ViewData["InvItemClass"] = await _context.InvItemClasses
                .Where(i => i.InvItemId == id)
                .Include(i => i.InvClassification)
                .ToListAsync();

            return Page();
        }
    }
}
