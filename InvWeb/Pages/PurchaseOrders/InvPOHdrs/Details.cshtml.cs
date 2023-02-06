using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.PurchaseOrders.InvPOHdrs
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public InvPoHdr InvPoHdr { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvPoHdr = await _context.InvPoHdrs
                .Include(i => i.InvStore)
                .Include(i => i.InvSupplier)
                .Include(i => i.InvPoHdrStatu)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (InvPoHdr == null)
            {
                return NotFound();
            }

            ViewData["POItems"] = _context.InvPoItems.Where(i => i.InvPoHdrId == id)
                .Include(i => i.InvUom)
                .Include(i => i.InvItem)
                .ToList();

            return Page();
        }
    }
}
