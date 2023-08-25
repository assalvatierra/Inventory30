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
using Modules.Inventory;
using CoreLib.Inventory.Interfaces;
using CoreLib.DTO.Receiving;
using CoreLib.DTO.Common.TrxHeader;

namespace InvWeb.Pages.Stores.Adjustment
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IItemTrxServices itemTrxServices;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ApplicationDbContext context, ILogger<IndexModel> logger)
        {
            _logger = logger;
            _context = context;
            itemTrxServices = new ItemTrxServices(_context, _logger);
        }

        public IList<InvTrxHdr> InvTrxHdr { get;set; }
        public TrxHeaderIndexModel AdjustmentIndexModel { get; set; }

        [BindProperty]
        public string Status { get; set; }   // filter Parameter
        [BindProperty]
        public string Orderby { get; set; }   //this is the key bit

        private readonly int TYPE_ADJUSTMENT = 3;

        public async Task<ActionResult> OnGetAsync(int? storeId, string status)
        {

            if (storeId == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(status))
            {
                status = "PENDING";
            }

            AdjustmentIndexModel = await itemTrxServices.GetTrxHeaderIndexModel_OnGetAsync(InvTrxHdr, (int)storeId, TYPE_ADJUSTMENT, status, IsUserAdmin());

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(int? storeId)
        {
            if (storeId == null)
            {
                return NotFound();
            }

            AdjustmentIndexModel = await itemTrxServices.GetTrxHeaderIndexModel_OnPostAsync(InvTrxHdr, (int)storeId, TYPE_ADJUSTMENT, Status, Orderby,IsUserAdmin());


            return Page();
        }

        private bool IsUserAdmin()
        {
            return User.IsInRole("Admin");
        }
    }
}
