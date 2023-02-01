using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Stores.Adjustment
{
    public class IndexModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public IndexModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvTrxHdr> InvTrxHdr { get;set; }

        [BindProperty]
        public string Status { get; set; }   // filter Parameter
        [BindProperty]
        public string Orderby { get; set; }   //this is the key bit

        private readonly int TYPE_ADJUSTMENT = 3;

        public async Task<ActionResult> OnGetAsync(int? storeId, string status)
        {

            if (storeId == null)
            {
                return NotFound();
            }

            InvTrxHdr = await _context.InvTrxHdrs
                .Include(i => i.InvStore)
                .Include(i => i.InvTrxHdrStatu)
                .Include(i => i.InvTrxType)
                .Include(i => i.InvTrxDtls)
                    .ThenInclude(i => i.InvItem)
                    .ThenInclude(i => i.InvUom)
                .Where(i => i.InvStoreId == storeId && i.InvTrxTypeId == 3)
                .ToListAsync();

            if (!String.IsNullOrWhiteSpace(status))
            {
                InvTrxHdr = status switch
                {
                    "PENDING" => InvTrxHdr.Where(i => i.InvTrxHdrStatusId == 1).ToList(),
                    "ACCEPTED" => InvTrxHdr.Where(i => i.InvTrxHdrStatusId == 2).ToList(),
                    "ALL" => InvTrxHdr.ToList(),
                    _ => InvTrxHdr.Where(i => i.InvTrxHdrStatusId == 1).ToList(),
                };
            }
            else
            {
                InvTrxHdr.Where(i => i.InvTrxHdrStatusId == 1).ToList();
            }

            ViewData["StoreId"] = storeId;
            ViewData["Status"] = status;
            ViewData["IsAdmin"] = User.IsInRole("ADMIN");

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(int? storeId)
        {
            if (storeId == null)
            {
                return NotFound();
            }

            InvTrxHdr = await _context.InvTrxHdrs
                .Include(i => i.InvStore)
                .Include(i => i.InvTrxHdrStatu)
                .Include(i => i.InvTrxType)
                  .Where(i => i.InvTrxTypeId == TYPE_ADJUSTMENT &&
                              i.InvStoreId == storeId)
                .ToListAsync();

            if (!String.IsNullOrWhiteSpace(Status))
            {
                InvTrxHdr = Status switch
                {
                    "PENDING"  => InvTrxHdr.Where(i => i.InvTrxHdrStatusId == 1).ToList(),
                    "ACCEPTED" => InvTrxHdr.Where(i => i.InvTrxHdrStatusId == 2).ToList(),
                    "ALL"      => InvTrxHdr.ToList(),
                    _          => InvTrxHdr.Where(i => i.InvTrxHdrStatusId == 1).ToList(),
                };
            }

            if (!String.IsNullOrWhiteSpace(Orderby))
            {
                InvTrxHdr = Orderby switch
                {
                    "ASC"  => InvTrxHdr.OrderBy(c => c.DtTrx).ToList(),
                    "DESC" => InvTrxHdr.OrderByDescending(c => c.DtTrx).ToList(),
                    _      => InvTrxHdr.OrderBy(c => c.DtTrx).ToList(),
                };
            }
            ViewData["StoreId"] = storeId;
            ViewData["IsAdmin"] = User.IsInRole("ADMIN");

            return Page();
        }
    }
}
