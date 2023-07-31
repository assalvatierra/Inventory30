using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Masterfiles.ItemSpecSteel.SteelOrigins
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

      public SteelOrigin SteelOrigin { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.SteelOrigins == null)
            {
                return NotFound();
            }

            var steelorigin = await _context.SteelOrigins.FirstOrDefaultAsync(m => m.Id == id);
            if (steelorigin == null)
            {
                return NotFound();
            }
            else 
            {
                SteelOrigin = steelorigin;
            }
            return Page();
        }
    }
}
