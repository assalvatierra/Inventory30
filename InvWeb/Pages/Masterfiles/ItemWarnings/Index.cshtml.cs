using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Masterfiles.ItemWarnings
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvWarningLevel> InvWarningLevel { get;set; }

        public async Task OnGetAsync()
        {
            InvWarningLevel = await _context.InvWarningLevels
                .Include(i => i.InvItem)
                .Include(i => i.InvUom)
                .Include(i => i.InvWarningType)
                .ToListAsync();
        }
    }
}
