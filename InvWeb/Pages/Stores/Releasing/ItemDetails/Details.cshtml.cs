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

namespace InvWeb.Pages.Stores.Releasing.ItemDetails
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DeleteModel> _logger;
        private readonly IItemDtlsServices _itemDtlsServices;

        public DetailsModel(ILogger<DeleteModel> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
            _itemDtlsServices = new ItemDtlsServices(context, _logger);
        }

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
    }
}
