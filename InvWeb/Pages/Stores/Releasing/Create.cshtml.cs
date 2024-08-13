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
using CoreLib.DTO.Releasing;
using CoreLib.Interfaces;
using Inventory;

namespace InvWeb.Pages.Stores.Releasing
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;
        private readonly IItemTrxServices itemTrxServices;
        private readonly IStoreServices storeServices;
        private readonly IInvApprovalServices invApprovalServices;

        public CreateModel(ILogger<CreateModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            itemTrxServices = new ItemTrxServices(_context, _logger);
            storeServices = new StoreServices(_context, _logger);
            invApprovalServices = new InvApprovalServices(_context, _logger);
        }

        [BindProperty]
        public ReleasingCreateEditModel ReleasingCreateModel { get; set; }
        public InvTrxHdr InvTrxHdr;
        public int StoreId { get; set; }
        //private int STATUS_RELEASED = 2;

        public IActionResult OnGet(int? storeId)
        {
            if (storeId == null)
            {
                return NotFound();
            }

            this.UpdateStoreId((int)storeId);

            ReleasingCreateModel = itemTrxServices.GetReleasingCreateModel_OnCreateOnGet(InvTrxHdr, StoreId, GetUser(), GetStores());
            
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ReleasingCreateModel = itemTrxServices.GetReleasingCreateModel_OnCreateOnGet(ReleasingCreateModel.InvTrxHdr, StoreId, GetUser(), GetStores());
                return Page();
            }

            itemTrxServices.CreateInvTrxHdrs(ReleasingCreateModel.InvTrxHdr);
            await itemTrxServices.SaveChanges();

            await CreateTrxHdrApproval(ReleasingCreateModel.InvTrxHdr.Id);

            return RedirectToPage("./Form", new { id = ReleasingCreateModel.InvTrxHdr.Id});
        }

        private string GetUser()
        {
            return User.FindFirstValue(ClaimTypes.Name);
        }

        private List<InvStore> GetStores()
        {
            return storeServices.GetInvStores().ToList();
        }

        private void UpdateStoreId(int storeId)
        {
            StoreId = storeId;
        }


        private async Task CreateTrxHdrApproval(int invTrxHdrId)
        {
            var newTrxHdrApproval = new InvTrxApproval();
            newTrxHdrApproval.EncodedBy = GetUser();
            newTrxHdrApproval.EncodedDate = DateTime.UtcNow;
            newTrxHdrApproval.InvTrxHdrId = invTrxHdrId;

            //invApprovalServices.CreateTrxApproval(newTrxHdrApproval);
            _context.InvTrxApprovals.Add(newTrxHdrApproval);
            _context.SaveChanges();

            await invApprovalServices.SaveChangesAsync();
        }

    }
}
