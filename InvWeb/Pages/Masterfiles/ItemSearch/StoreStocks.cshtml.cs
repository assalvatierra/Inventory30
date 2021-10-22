using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.StoreStock
{
    public class StoreStocksModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public StoreStocksModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvTrxDtl> InvTrxDtls { get;set; }

        public async Task OnGetAsync(int id)
        {
            InvTrxDtls = await _context.InvTrxDtls
                .Where(i => i.InvItemId == id && i.InvTrxHdr.InvTrxHdrStatusId > 1)
                .Include(i => i.InvItem)
                .Include(i => i.InvTrxHdr.InvStore)
                .Include(i => i.InvUom)
                .ToListAsync();
        }
    }
}
