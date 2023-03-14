using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreLib.Inventory.Models;
using System.Security.Claims;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Interfaces;
using Microsoft.Extensions.Logging;
using Modules.Inventory;
using CoreLib.DTO.Receiving;
using CoreLib.DTO.Common.TrxHeader;

namespace InvWeb.Pages.Stores.Adjustment
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;
        private readonly IItemTrxServices itemTrxServices;

        public CreateModel(ApplicationDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
            itemTrxServices = new ItemTrxServices(_context, _logger);
        }

        [BindProperty]
        public TrxHeaderCreateEditModel AdjustmentCreateModel { get; set; }
        public InvTrxHdr InvTrxHdr;
        public int StoreId { get; set; }
        private int STATUS_ADJUSTMENT = 3;

        public IActionResult OnGet(int? storeId)
        {

            AdjustmentCreateModel = itemTrxServices.GetTrxHeaderCreateModel_OnCreateOnGet(InvTrxHdr, (int)storeId, GetUser());
            AdjustmentCreateModel.InvTrxHdr.InvTrxTypeId = STATUS_ADJUSTMENT;

            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                AdjustmentCreateModel = itemTrxServices.GetTrxHeaderCreateModel_OnCreateOnGet(AdjustmentCreateModel.InvTrxHdr, StoreId, GetUser());
                return Page();
            }

            itemTrxServices.CreateInvTrxHdrs(AdjustmentCreateModel.InvTrxHdr);
            await itemTrxServices.SaveChanges();

            // _context.InvTrxHdrs.Add(InvTrxHdr);
            // await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { storeId = AdjustmentCreateModel.InvTrxHdr.InvStoreId });
        }

        private string GetUser()
        {
            return User.FindFirstValue(ClaimTypes.Name);
        }
        private void GetStoreId(int storeId)
        {
             StoreId = storeId;
        }
    }
}
