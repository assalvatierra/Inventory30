using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Interfaces;
using Microsoft.Extensions.Logging;
using Modules.Inventory;
using CoreLib.DTO.Common.TrxHeader;

namespace InvWeb.Pages.Stores.Adjustment
{
    public class CancelModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DeleteModel> _logger;
        private readonly IItemTrxServices itemTrxServices;


        public CancelModel(ILogger<DeleteModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            itemTrxServices = new ItemTrxServices(_context, _logger);
        }


        [BindProperty]
        public InvTrxHdr InvTrxHdr { get; set; }

        //public InvTrxHdr InvTrxHdr { get; set; }

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
                InvTrxHdr.InvTrxHdrStatusId = 4;

                itemTrxServices.EditInvTrxHdrs(InvTrxHdr);

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new { storeId = InvTrxHdr.InvStoreId, Status="PENDING" });
        }
    }
}
