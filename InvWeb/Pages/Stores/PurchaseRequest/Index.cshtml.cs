using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.Stores.PurchaseRequest
{
    public class IndexModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public IndexModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvPoHdr> InvPoHdr { get;set; }

        public async Task<IActionResult> OnGetAsync(int? storeId, string status)
        {
            if (storeId == null)
            {
                return NotFound();
            }

            InvPoHdr = await _context.InvPoHdrs
                .Include(i => i.InvPoHdrStatu)
                .Include(i => i.InvStore)
                .Include(i => i.InvSupplier)
                .Include(i => i.InvPoItems)
                    .ThenInclude(i => i.InvItem)
                    .ThenInclude(i => i.InvUom)
                  .Where(i => i.InvStoreId == storeId)
                .ToListAsync();

            if (!String.IsNullOrWhiteSpace(status))
            {
                InvPoHdr = status switch
                {
                    "PENDING" => InvPoHdr.Where(i => i.InvPoHdrStatusId == 1).ToList(),
                    "ACCEPTED" => InvPoHdr.Where(i => i.InvPoHdrStatusId == 2).ToList(),
                    "ALL" => InvPoHdr.ToList(),
                    _ => InvPoHdr.Where(i => i.InvPoHdrStatusId == 1).ToList(),
                };
            }

            ViewData["StoreId"] = storeId;
            ViewData["Status"] = status;
            return Page();
        }
    }
}
