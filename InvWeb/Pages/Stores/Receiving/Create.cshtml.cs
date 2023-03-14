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
using CoreLib.DTO.Releasing;
using CoreLib.Inventory.Interfaces;
using Microsoft.Extensions.Logging;
using Modules.Inventory;
using CoreLib.DTO.Receiving;

namespace InvWeb.Pages.Stores.Receiving
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
        public ReceivingCreateEditModel ReceivingCreateModel { get; set; }
        public InvTrxHdr InvTrxHdr;
        public int StoreId { get; set; }
        //private int STATUS_RECEIVING = 1;

        public IActionResult OnGet(int? storeId)
        {
            if (storeId == null)
            {
                return NotFound();
            }

            this.UpdateStoreId((int)storeId);

            ReceivingCreateModel = itemTrxServices.GetReceivingCreateModel_OnCreateOnGet(InvTrxHdr, StoreId, GetUser());

            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ReceivingCreateModel = itemTrxServices.GetReceivingCreateModel_OnCreateOnGet(InvTrxHdr, StoreId, GetUser());
                return Page();
            }

            //_context.InvTrxHdrs.Add(ReceivingCreateModel.InvTrxHdr);

            itemTrxServices.CreateInvTrxHdrs(ReceivingCreateModel.InvTrxHdr);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Details", new { id = ReceivingCreateModel.InvTrxHdr.Id });
        }

        private string GetUser()
        {
            return User.FindFirstValue(ClaimTypes.Name);
        }


        private void UpdateStoreId(int storeId)
        {
            StoreId = storeId;
        }

    }
}
