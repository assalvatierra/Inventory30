using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Masterfiles.ItemCustomSpec
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvCustomSpec> InvCustomSpec { get;set; }

        public async Task OnGetAsync()
        {
            InvCustomSpec = await _context.InvCustomSpecs
                .Include(i => i.InvCustomSpecType).ToListAsync();
        }
    }
}
