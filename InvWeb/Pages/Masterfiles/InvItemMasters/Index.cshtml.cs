using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLib.Models.Inventory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InvWeb.Pages.Masterfiles.InvItemMasters
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvItemMaster> InvItemMaster { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.InvItemMasters != null)
            {
                InvItemMaster = await _context.InvItemMasters
                .Include(i => i.InvItem)
                .Include(i => i.InvItemBrand)
                .Include(i => i.InvItemOrigin)
                .Include(i => i.InvUom).ToListAsync();
            }
        }
    }
}
