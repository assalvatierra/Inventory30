using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.ItemPO
{
    public class IndexModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public IndexModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvPoHdr> InvPoHdr { get;set; }

        public async Task OnGetAsync()
        {
            InvPoHdr = await _context.InvPoHdrs
                .Include(i => i.InvStore)
                .Include(i => i.InvSupplier).ToListAsync();
        }
    }
}
