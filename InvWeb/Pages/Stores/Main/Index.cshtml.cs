using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvWeb.Data;
using InvWeb.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebDBSchema.Models;
using WebDBSchema.Models.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace InvWeb.Pages.Stores.Main
{
    [Authorize(Roles = "ADMIN,STORE")]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly StoreServices _storeSvc;

        public IndexModel(ILogger<IndexModel> logger,ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            _storeSvc = new StoreServices(context);
        }

        public InvStore InvStore { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {

                if (id == null)
                {
                    return NotFound();
                }

                InvStore = await _context.InvStores
                    .Where(s => s.Id == id)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (InvStore == null)
                {
                    return NotFound();
                }

                var storeId = (int)id;

                ViewData["StoreId"] = id;
                ViewData["StoreInv"] = await _storeSvc.GetStoreItemsSummary(storeId);
                ViewData["PendingReceiving"] = await _storeSvc.GetReceivingPendingAsync(storeId);
                ViewData["PendingReleasing"] = await _storeSvc.GetReleasingPendingAsync(storeId);
                ViewData["PendingAdjustment"] = await _storeSvc.GetAdjustmentPendingAsync(storeId);
                ViewData["PendingPurchaseOrder"] = await _storeSvc.GetPurchaseOrderPendingAsync(storeId);
                ViewData["RecentTrxHdrs"] = await _storeSvc.GetRecentTransactions(storeId);

                _logger.LogInformation("Showing Store Main Page - StoreID : " + id);

                return Page();

            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
        }
    }
}
