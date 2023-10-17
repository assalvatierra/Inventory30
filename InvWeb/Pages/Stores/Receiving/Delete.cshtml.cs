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

namespace InvWeb.Pages.Stores.Receiving
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DeleteModel> _logger;
        private readonly IItemTrxServices itemTrxServices;


        public DeleteModel(ILogger<DeleteModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            itemTrxServices = new ItemTrxServices(_context, _logger);
        }


        [BindProperty]
        public InvTrxHdr InvTrxHdr { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvTrxHdr = await itemTrxServices.GetInvTrxHdrsById((int)id).FirstOrDefaultAsync();

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

            InvTrxHdr = await itemTrxServices.GetInvTrxHdrsByIdAsync((int)id);

            if (InvTrxHdr != null)
            {
                await itemTrxServices.DeleteInvTrxHdrs_AndTrxDtlsItems(InvTrxHdr);
                await itemTrxServices.SaveChanges();
            }

            return RedirectToPage("./Index", new { storeId = InvTrxHdr.InvStoreId, status = "PENDING" });
        }


        private bool DeleteSteeItems(InvTrxHdr invTrxHdr)
        {
            if (invTrxHdr.InvTrxDtls != null)
            {
                var invitems = _context.InvTrxDtls.Where(c => c.InvTrxHdrId == invTrxHdr.Id).ToList();

                _context.InvTrxDtls.RemoveRange(invitems);

                return true;
            }
            return false;
        }
    }
}
