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
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
