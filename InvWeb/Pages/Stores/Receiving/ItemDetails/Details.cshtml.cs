using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Stores.Receiving.ItemDetails
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public InvTrxDtl InvTrxDtl { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvTrxDtl = await _context.InvTrxDtls
                .Include(i => i.InvItem)
                .Include(i => i.InvTrxHdr)
                .Include(i => i.InvUom).FirstOrDefaultAsync(m => m.Id == id);

            if (InvTrxDtl == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
