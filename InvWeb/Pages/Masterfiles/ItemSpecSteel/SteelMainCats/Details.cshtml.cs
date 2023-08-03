using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Masterfiles.ItemSpecSteel.SteelMainCats
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

      public SteelMainCat SteelMainCat { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.SteelMainCats == null)
            {
                return NotFound();
            }

            var steelmaincat = await _context.SteelMainCats.FirstOrDefaultAsync(m => m.Id == id);
            if (steelmaincat == null)
            {
                return NotFound();
            }
            else 
            {
                SteelMainCat = steelmaincat;
            }
            return Page();
        }
    }
}
