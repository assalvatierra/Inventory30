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
using CoreLib.DTO.Common.TrxDetails;

namespace InvWeb.Pages.Stores.Receiving.ItemDetails
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DetailsModel> _logger;
        private readonly IItemDtlsServices _itemDtlsServices;

        public DetailsModel(ApplicationDbContext context, ILogger<DetailsModel> logger)
        {
            _context = context;
            _logger = logger;
            _itemDtlsServices = new ItemDtlsServices(_context, _logger);
        }

        public TrxDetailsItemDetailsModel TrxDetailsItem { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TrxDetailsItem = await _itemDtlsServices.GetTrxDetailsModel_OnDetailsAsync((int)id);
 
            if (TrxDetailsItem.InvTrxDtl == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
