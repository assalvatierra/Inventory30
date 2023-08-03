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
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<SteelBrand> SteelBrand { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.SteelBrands != null)
            {
                SteelBrand = (IList<SteelBrand>)await _context.SteelBrands.ToListAsync();
            }
        }
    }
}
