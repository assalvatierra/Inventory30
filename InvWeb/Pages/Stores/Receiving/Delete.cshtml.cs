using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Stores.Receiving
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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
                .Include(i => i.InvTrxType).FirstOrDefaultAsync(m => m.Id == id);

            if (InvTrxHdr == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvTrxHdr = await _context.InvTrxHdrs.FindAsync(id);

            if (InvTrxHdr != null)
            {
                //remove transactions detail items
                var itemList = await _context.InvTrxDtls.Where(i => i.InvTrxHdrId == InvTrxHdr.Id).ToListAsync();
                _context.InvTrxDtls.RemoveRange(itemList);

                _context.InvTrxHdrs.Remove(InvTrxHdr);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new { storeId = InvTrxHdr.InvStoreId, status = "PENDING" });
        }
    }
}
