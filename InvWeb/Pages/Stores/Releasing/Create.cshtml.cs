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

namespace InvWeb.Pages.Stores.Releasing
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;
        private readonly IItemTrxServices itemTrxServices;
        private readonly IStoreServices storeServices;

        public CreateModel(ILogger<CreateModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            itemTrxServices = new ItemTrxServices(_context, _logger);
            storeServices = new StoreServices(_context, _logger);
        }

        [BindProperty]
        public InvTrxHdr InvTrxHdr { get; set; }
        public ReleasingCreateEditModel ReleasingCreateModel { get; set; }
        public int StoreId { get; set; }


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

            itemTrxServices.CreateInvTrxHdrs(InvTrxHdr);
            await itemTrxServices.SaveChanges();

            return RedirectToPage("./Details", new { id = InvTrxHdr.Id});
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
    }
}
