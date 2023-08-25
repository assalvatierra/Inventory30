using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using CoreLib.DTO.Common.TrxHeader;
using CoreLib.Inventory.Interfaces;
using Microsoft.Extensions.Logging;
using Modules.Inventory;
using CoreLib.DTO.PurchaseOrder;
using CoreLib.Interfaces;
using Inventory;

namespace InvWeb.Pages.Stores.PurchaseRequest
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IInvPOHdrServices invPOHdrServices;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ApplicationDbContext context, ILogger<IndexModel> logger)
        {
            _logger = logger;
            _context = context;
            invPOHdrServices = new InvPOHdrServices(_context, _logger);
        }

        public IList<InvPoHdr> InvPoHdr { get; set; }
        public InvPOHdrModel InvPOHdrModel { get; set; }

        [BindProperty]
        public string Status { get; set; }   // filter Parameter
        [BindProperty]
        public string Orderby { get; set; }   //this is the key bit


        public async Task<IActionResult> OnGetAsync(int? storeId, string status)
        {
            if (storeId == null)
            {
                return NotFound();
            }

            InvPOHdrModel = await invPOHdrServices.GetInvPOHdrModel_OnIndex(InvPoHdr, (int)storeId, status, IsUserAdminRole());

            ViewData["StoreId"] = storeId;
            ViewData["Status"] = status;
            ViewData["IsAdmin"] = User.IsInRole("Admin");
            return Page();
        }

        private bool IsUserAdminRole()
        {
            return User.IsInRole("Admin");
        }
    }
}
