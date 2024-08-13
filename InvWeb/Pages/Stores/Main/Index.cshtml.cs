using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvWeb.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Inventory.Models.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreLib.Inventory.Interfaces;
using CoreLib.Models.Inventory;
using Modules.Inventory;

namespace InvWeb.Pages.Stores.Main
{
    [Authorize(Roles = "Admin,Store,Accounting,Procurement-head,Purchaser")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;
        private readonly IStoreServices _storeSvc;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            _storeSvc = new StoreServices(_context, logger);
        }

        public InvStore InvStore { get; set; }
        public int SelectedFilterCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, int? categoryId, string sort, string search)
        {
            try
            {

                if (id == null)
                {
                    return NotFound();
                }


                if (categoryId == null)
                {
                    categoryId = 0;
                }


                if (CheckUserLogin())
                {
                    RedirectToAction("../../Shared/LoginPartial");
                };

                InvStore = await _storeSvc.GetStorebyIdAsync((int)id);

                if (InvStore == null)
                {
                    return NotFound();
                }

                var storeId = (int)id;

                ViewData["StoreId"] = id;
                ViewData["StoreInv"] = await _storeSvc.GetStoreItemsSummary(storeId, (int)categoryId, sort, search);
                ViewData["PendingReceiving"] = await _storeSvc.GetReceivingPendingAsync(storeId);
                ViewData["PendingReleasing"] = await _storeSvc.GetReleasingPendingAsync(storeId);
                ViewData["PendingAdjustment"] = await _storeSvc.GetAdjustmentPendingAsync(storeId);
                ViewData["PendingPurchaseOrder"] = await _storeSvc.GetPurchaseOrderPendingAsync(storeId);
                ViewData["RecentTrxHdrs"] = await _storeSvc.GetRecentTransactions(storeId);
                ViewData["Categories"] = await _storeSvc.GetCategoriesList();
                ViewData["Category"] = categoryId;
                ViewData["Sort"] = sort;
                ViewData["SearchStr"] = search;

                _logger.LogInformation("Showing Store Main Page - StoreID : " + id);

                HttpContext.Session.SetString("Sample Session", "Sample");

                ViewData["IsAdmin"] = User.IsInRole("Admin"); // TOOD: check if user is admin
                return Page();

            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
        }

        public bool CheckUserLogin()
        {
            List<InvStore> UsersStores;
            var user = User.FindFirstValue(ClaimTypes.Name);

            if (!String.IsNullOrWhiteSpace(user))
            {
                UsersStores = _storeSvc.GetStoreUsers(user);

                if (UsersStores.FirstOrDefault() != null)
                {
                    HttpContext.Session.SetInt32("_UsersStoreId", UsersStores.FirstOrDefault().Id);

                    if (UsersStores.Count() > 0)
                    {
                        var userIds = UsersStores.Select(c => new { c.Id, c.StoreName }).ToList();
                        var jsonarr = JsonConvert.SerializeObject(userIds);

                        HttpContext.Session.SetString("_CurrentUserStores", jsonarr);
                    }
                }

                return true;
            }

            return false;
        }

      

    }
}
