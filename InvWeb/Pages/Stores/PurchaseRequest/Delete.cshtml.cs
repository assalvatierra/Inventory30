using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using CoreLib.DTO.PurchaseOrder;
using CoreLib.Interfaces;
using Inventory;
using Microsoft.Extensions.Logging;

namespace InvWeb.Pages.Stores.PurchaseRequest
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IInvPOHdrServices invPOHdrServices;
        private readonly ILogger<IndexModel> _logger;

        public DeleteModel(ApplicationDbContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
            invPOHdrServices = new InvPOHdrServices(_context, _logger);
        }

        [BindProperty]
        public InvPOHdrDeleteModel InvPoHdrDelete { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvPoHdrDelete.InvPoHdr = await invPOHdrServices.GetInvPoHdrsbyIdAsync((int)id);

            if (InvPoHdrDelete.InvPoHdr == null)
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

            InvPoHdrDelete.InvPoHdr = await invPOHdrServices.InvPOHdrDelete_FindByIdAsync((int)id);

            if (InvPoHdrDelete.InvPoHdr != null)
            {
                //remove transactions detail items
                invPOHdrServices.RemoveInvPOHdrDeleteModel(InvPoHdrDelete);

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new { storeId = InvPoHdrDelete.InvPoHdr.InvStoreId });
        }
    }
}
