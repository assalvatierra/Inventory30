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

namespace InvWeb.Pages.Stores.Releasing
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
                //remove transactions detail items
                itemTrxServices.RemoveTrxDtlsList(InvTrxHdr.Id);

                //remove header
                itemTrxServices.DeleteInvTrxHdrs(InvTrxHdr);
                await itemTrxServices.SaveChanges();
            }

            return RedirectToPage("./Index", new { storeId = InvTrxHdr.InvStoreId, status = "PENDING" });
        }
    }
}
