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

namespace InvWeb.Pages.Stores.Releasing
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;
        private readonly IItemTrxServices itemTrxServices;

        public IndexModel(ApplicationDbContext context, ILogger<IndexModel> logger)
        {
            _logger = logger;
            _context = context;
            itemTrxServices = new ItemTrxServices(_context, _logger);
        }

        public IList<InvTrxHdr> InvTrxHdr { get;set; }
        public ReleasingIndexModel ReleasingIndexModel { get; set; }

        private readonly int TYPE_RELEASING = 2;

        public async Task OnGetAsync(int? storeId, string status)
        {
            if (storeId == null)
            {
                //return NotFound();
            }

            if (!string.IsNullOrEmpty(status))
            {
                Status = status;
            }

            ReleasingIndexModel = await itemTrxServices.GetReleasingIndexModel_OnIndexOnGetAsync(
                                           InvTrxHdr, (int)storeId, TYPE_RELEASING, status, IsUserRoleAdmin());

        }


        [BindProperty]
        public string Status { get; set; }   // filter Parameter
        [BindProperty]
        public string Orderby { get; set; }   //this is the key bit

        public async Task<IActionResult> OnPostAsync(int? storeId)
        {
            if (storeId == null)
            {
                return NotFound();
            }

            ReleasingIndexModel = await itemTrxServices.GetReleasingIndexModel_OnIndexOnPostAsync(
                                           InvTrxHdr, (int)storeId, TYPE_RELEASING, Status, Orderby, IsUserRoleAdmin());
            return Page();
        }

        private bool IsUserRoleAdmin()
        {
            return User.IsInRole("ADMIN");
        }
    }
}
