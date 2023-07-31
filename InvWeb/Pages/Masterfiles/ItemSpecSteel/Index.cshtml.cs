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
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvItemSpec_Steel> InvItemSpec_Steel { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.InvItemSpec_Steel != null)
            {
                InvItemSpec_Steel = await _context.InvItemSpec_Steel
                .Include(i => i.InvItem)
                .Include(i => i.SteelBrand)
                .Include(i => i.SteelMainCat)
                .Include(i => i.SteelMaterial)
                .Include(i => i.SteelMaterialGrade)
                .Include(i => i.SteelOrigin)
                .Include(i => i.SteelSubCat).ToListAsync();
            }
        }
    }
}
