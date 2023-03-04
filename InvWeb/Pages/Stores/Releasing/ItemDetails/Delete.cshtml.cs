using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using Microsoft.Extensions.Logging;
using CoreLib.Inventory.Interfaces;
using Modules.Inventory;

namespace InvWeb.Pages.Stores.Releasing.ItemDetails
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DeleteModel> _logger;
        private readonly IItemDtlsServices _itemDtlsServices;

        public DeleteModel(ILogger<DeleteModel> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
            _itemDtlsServices = new ItemDtlsServices(context, _logger);
        }

        [BindProperty]
        public InvTrxDtl InvTrxDtl { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvTrxDtl = await _itemDtlsServices.GetInvDtlsById((int)id)
                                .FirstOrDefaultAsync();

            if (InvTrxDtl == null)
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

            InvTrxDtl = await _itemDtlsServices.GetInvDtlsByIdAsync((int)id);

            if (InvTrxDtl != null)
            {
                _itemDtlsServices.DeleteInvDtls(InvTrxDtl);
                await _itemDtlsServices.SaveChangesAsync();
            }

            return RedirectToPage("../Details", new { id = InvTrxDtl.InvTrxHdrId });
        }
    }
}
