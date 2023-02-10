using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Masterfiles.ItemMaster.Categories
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public InvCategory InvCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvCategory = await _context.InvCategories
                .Include(i=>i.InvCategorySpecDefs)
                    .ThenInclude(i=>i.InvItemSysDefinedSpec)
                .Include(i => i.InvCatCustomSpecs)
                    .ThenInclude(i => i.InvCustomSpec)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (InvCategory == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
