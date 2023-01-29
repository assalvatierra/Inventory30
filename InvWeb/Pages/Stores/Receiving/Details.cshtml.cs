using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Stores.Receiving
{
    public class DetailsModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public DetailsModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public InvTrxHdr InvTrxHdr { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvTrxHdr = await _context.InvTrxHdrs
                .Include(i => i.InvStore)
                .Include(i => i.InvTrxHdrStatu)
                .Include(i => i.InvTrxDtls)
                .Include(i => i.InvTrxType).FirstOrDefaultAsync(m => m.Id == id);

            ViewData["InvTrxDtls"] = await _context.InvTrxDtls.Where(i => i.InvTrxHdrId == id)
                .Include(i => i.InvItem)
                .Include(i => i.InvUom)
                .ToListAsync();

            if (InvTrxHdr == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
