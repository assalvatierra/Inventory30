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
                           && i.InvStoreId   == storeId
                           && i.InvTrxHdrStatusId == 1)
                .Include(i => i.InvTrxType).ToListAsync();

            ViewData["StoreId"] = storeId;
        }


        [BindProperty]
        public string status { get; set; }   // filter Parameter
        [BindProperty]
        public string orderby { get; set; }   //this is the key bit

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
                  .Where(i => i.InvTrxTypeId == TYPE_RELEASING &&
                              i.InvStoreId == storeId)
                .ToListAsync();

            if (!String.IsNullOrWhiteSpace(status))
            {
                switch (status)
                {
                    case "PENDING":
                        InvTrxHdr = InvTrxHdr.Where(i => i.InvTrxHdrStatusId == 1).ToList();
                        break;
                    case "ACCEPTED":
                        InvTrxHdr = InvTrxHdr.Where(i => i.InvTrxHdrStatusId == 2).ToList();
                        break;
                    case "ALL":
                        InvTrxHdr = InvTrxHdr.ToList();
                        break;
                    default:
                        InvTrxHdr = InvTrxHdr.Where(i => i.InvTrxHdrStatusId == 1).ToList();
                        break;
                }
            }

            if (!String.IsNullOrWhiteSpace(orderby))
            {
                switch (orderby)
                {
                    case "ASC":
                        InvTrxHdr = InvTrxHdr.OrderBy(c => c.DtTrx).ToList();
                        break;
                    case "DESC":
                        InvTrxHdr = InvTrxHdr.OrderByDescending(c => c.DtTrx).ToList();
                        break;
                    default:
                        InvTrxHdr = InvTrxHdr.OrderBy(c => c.DtTrx).ToList();
                        break;
                }
            }

            ViewData["StoreId"] = storeId;

            return Page();
        }
    }
}
