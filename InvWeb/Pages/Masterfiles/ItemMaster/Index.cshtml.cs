using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.ItemMaster
{
    public class IndexModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public IndexModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvItem> InvItem { get;set; }

        public async Task OnGetAsync()
        {
            InvItem = await _context.InvItems
                .Include(i => i.InvUom).ToListAsync();
        }
    }
}
