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
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InvWeb.Pages.Stores.Main
{
    [Authorize(Roles = "ADMIN,STORE")]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly StoreServices _storeSvc;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            _storeSvc = new StoreServices(context, logger);
        }

        public InvStore InvStore { get; set; }

        [BindProperty(SupportsGet = true)]
        public string OrderBy { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, string orderBy)
        {
            try
            {

                if (id == null)
                {
                    return NotFound();
                }

                if (!String.IsNullOrWhiteSpace(OrderBy))
                {
                    OrderBy = orderBy;
                }

                if (CheckUserLogin())
                {
                    RedirectToAction("../../Shared/LoginPartial");
                };

                InvStore = await _context.InvStores
                    .Where(s => s.Id == id)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (InvStore == null)
                {
                    return NotFound();
                }

                var storeId = (int)id;

                ViewData["StoreId"] = id;
                ViewData["StoreInv"] = await _storeSvc.GetStoreItemsSummary(storeId, OrderBy);
                ViewData["PendingReceiving"] = await _storeSvc.GetReceivingPendingAsync(storeId);
                ViewData["PendingReleasing"] = await _storeSvc.GetReleasingPendingAsync(storeId);
                ViewData["PendingAdjustment"] = await _storeSvc.GetAdjustmentPendingAsync(storeId);
                ViewData["PendingPurchaseOrder"] = await _storeSvc.GetPurchaseOrderPendingAsync(storeId);
                ViewData["RecentTrxHdrs"] = await _storeSvc.GetRecentTransactions(storeId);
                ViewData["OrderByList"] = GetOrderByList();

                _logger.LogInformation("Showing Store Main Page - StoreID : " + id);

                if (!String.IsNullOrEmpty(OrderBy))
                {
                    _logger.LogInformation("Showing Store Main Page Order : " + OrderBy);
                }

                return Page();

            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
        }

        public static IEnumerable<SelectListItem> GetOrderByList()
        {
            IList<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem{Text = "Category", Value = "category"},
                new SelectListItem{Text = "Count", Value = "count"},
                new SelectListItem{Text = "Name", Value = "Name"},

            };
            return items;
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
