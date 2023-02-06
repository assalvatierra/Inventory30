using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Stores.Adjustment.ItemDetails
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvTrxDtl> InvTrxDtl { get;set; }

        public async Task OnGetAsync()
        {
            InvTrxDtl = await _context.InvTrxDtls
                .Include(i => i.InvItem)
                .Include(i => i.InvTrxHdr)
                .Include(i => i.InvUom).ToListAsync();
        }
    }
}
