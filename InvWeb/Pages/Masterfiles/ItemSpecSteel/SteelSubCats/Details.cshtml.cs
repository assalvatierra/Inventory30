using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Masterfiles.ItemSpecSteel.SteelSubCats
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

      public SteelSubCat SteelSubCat { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.SteelSubCats == null)
            {
                return NotFound();
            }

            var steelsubcat = await _context.SteelSubCats.FirstOrDefaultAsync(m => m.Id == id);
            if (steelsubcat == null)
            {
                return NotFound();
            }
            else 
            {
                SteelSubCat = steelsubcat;
            }
            return Page();
        }
    }
}
