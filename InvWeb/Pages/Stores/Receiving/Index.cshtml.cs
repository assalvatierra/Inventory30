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
using Modules.Inventory;
using Microsoft.Extensions.Logging;
using CoreLib.DTO.Releasing;
using CoreLib.DTO.Receiving;

namespace InvWeb.Pages.Stores.Receiving
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
        public ReceivingIndexModel ReceivingIndexModel { get; set; }

        private readonly int TYPE_RECEIVING = 1;

        public async Task<ActionResult> OnGetAsync(int? storeId, string status)
        {
            if (storeId == null)
            {
                return NotFound();
            }

            ReceivingIndexModel = await itemTrxServices.GetReceivingIndexModel_OnIndexOnGetAsync(InvTrxHdr, (int)storeId, TYPE_RECEIVING, Status, IsUserRoleAdmin());

            return Page();
        }

        [BindProperty]
        public string Status { get; set; }   //this is the key bit
        [BindProperty]
        public string Orderby { get; set; }   //this is the key bit

        public async Task<IActionResult> OnPostAsync(int? storeId)
        {
            if (storeId == null)
            {
                return NotFound();
            }

            ReceivingIndexModel = await itemTrxServices.GetReceivingIndexModel_OnIndexOnPostAsync(InvTrxHdr, (int)storeId, TYPE_RECEIVING, Status, Orderby, IsUserRoleAdmin());

            return Page();
        }

        private bool IsUserRoleAdmin()
        {
            return User.IsInRole("ADMIN");
        }
    }
}
