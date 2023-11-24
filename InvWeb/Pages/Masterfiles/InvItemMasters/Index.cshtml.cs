using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using Microsoft.AspNetCore.Authorization;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Masterfiles.InvItemMasters
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvItemMaster> InvItemMaster { get;set; }

        public async Task OnGetAsync()
        {
            InvItemMaster = await _context.InvItemMasters
                .Include(i=> i.InvItem)
                .Include(i => i.InvUom)
                .Include(i => i.InvItemBrand)
                .Include(i => i.InvItemOrigin)
                .ToListAsync();
        }
    }
}
