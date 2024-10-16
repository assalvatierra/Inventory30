﻿using InvWeb.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoreLib.Inventory.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using CoreLib.Inventory.Interfaces;
using CoreLib.Models.Inventory;
using Modules.Inventory;

namespace InvWeb.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IStoreServices _storeSvc;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _storeSvc = new StoreServices(context);

            this.syssetting = new InvWeb.Shared.SystemSetting();
            string sStore = this.syssetting.GetValue("Store");
            //string sAppName = label.GetValue("AppName");
            //string sAppVer = label.GetValue("AppVersion");
            //string sClient = label.GetValue("Tenant");

        }

        [BindProperty]
        public InvWeb.Shared.SystemSetting syssetting { get; set; }

        [BindProperty]
        public List<InvStore> UsersStores { get; set; }   //this is the key bit

        public void OnGet()
        {
            var userRole = User.IsInRole("Admin");
            try
            {
                var user = User.FindFirstValue(ClaimTypes.Name);

                if (user != null)
                {
                    UsersStores = _storeSvc.GetStoreUsers(user);

                    if (UsersStores.FirstOrDefault() != null)
                    {
                        HttpContext.Session.SetInt32("_UsersStoreId", UsersStores.FirstOrDefault().Id);

                        if (UsersStores.Count() > 0) {
                            var userIds = UsersStores.Select(c => new { c.Id, c.StoreName }).ToList();
                            var jsonarr = JsonConvert.SerializeObject(userIds);

                            HttpContext.Session.SetString("_CurrentUserStores", jsonarr);
                        }
                    }
                }

            }
            catch (Exception ex )
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
