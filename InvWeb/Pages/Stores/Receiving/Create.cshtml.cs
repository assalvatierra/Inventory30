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
using Inventory;
using CoreLib.Interfaces;

namespace InvWeb.Pages.Stores.Receiving
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;
        private readonly IItemTrxServices itemTrxServices;
        private readonly IInvApprovalServices invApprovalServices;
        private readonly DateServices dateServices;

        public CreateModel(ApplicationDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
            itemTrxServices = new ItemTrxServices(_context, _logger);
            invApprovalServices = new InvApprovalServices(_context, _logger);
            dateServices = new DateServices();
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
            ReceivingCreateModel.InvTrxHdr.DtTrx = dateServices.GetCurrentDateTime();

            itemTrxServices.CreateInvTrxHdrs(ReceivingCreateModel.InvTrxHdr);
            await _context.SaveChangesAsync();

            CreateTrxHdrApproval(ReceivingCreateModel.InvTrxHdr.Id);

            

            return RedirectToPage("./Form", new { id = ReceivingCreateModel.InvTrxHdr.Id });
        }

        private string GetUser()
        {
            return User.FindFirstValue(ClaimTypes.Name);
        }


        private void UpdateStoreId(int storeId)
        {
            StoreId = storeId;
        }


        private void CreateTrxHdrApproval(int invHdrId)
        {
            try
            {
                var newTrxHdrApproval = new InvTrxApproval();
                newTrxHdrApproval.EncodedBy = GetUser();
                newTrxHdrApproval.EncodedDate = dateServices.GetCurrentDateTime();
                newTrxHdrApproval.InvTrxHdrId = invHdrId;

                //invApprovalServices.CreateTrxApproval(newTrxHdrApproval);
                _context.InvTrxApprovals.Add(newTrxHdrApproval); 
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

    }
}
