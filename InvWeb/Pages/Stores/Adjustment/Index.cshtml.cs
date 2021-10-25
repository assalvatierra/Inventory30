using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;

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
        public string status { get; set; }   // filter Parameter

        private readonly int TYPE_ADJUSTMENT = 3;

        public async Task<ActionResult> OnGetAsync(int? storeId)
        {

            if (storeId == null)
            {
                return NotFound();
            }

            InvTrxHdr = await _context.InvTrxHdrs
                .Include(i => i.InvStore)
                .Include(i => i.InvTrxHdrStatu)
                .Where(i => i.InvStoreId == storeId && i.InvTrxTypeId == 3)
                .Include(i => i.InvTrxType).ToListAsync();

            ViewData["StoreId"] = storeId;


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

            ViewData["StoreId"] = storeId;

            return Page();
        }
    }
}
