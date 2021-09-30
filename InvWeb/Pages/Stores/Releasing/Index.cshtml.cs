using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.Stores.Releasing
{
    public class IndexModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public IndexModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvTrxHdr> InvTrxHdr { get;set; }

        private readonly int TYPE_RELEASING = 2;

        public async Task OnGetAsync(int storeId)
        {
            InvTrxHdr = await _context.InvTrxHdrs
                .Include(i => i.InvStore)
                .Include(i => i.InvTrxHdrStatu)
                  .Where(i => i.InvTrxTypeId == TYPE_RELEASING
                           && i.InvStoreId   == storeId)
                .Include(i => i.InvTrxType).ToListAsync();

            ViewData["StoreId"] = storeId;
        }
    }
}
