using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Masterfiles.ItemSpecSteel.SteelBrands
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

      public SteelBrand SteelBrand { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.SteelBrands == null)
            {
                return NotFound();
            }

            var steelbrand = await _context.SteelBrands.FirstOrDefaultAsync(m => m.Id == id);
            if (steelbrand == null)
            {
                return NotFound();
            }
            else 
            {
                SteelBrand = steelbrand;
            }
            return Page();
        }
    }
}
