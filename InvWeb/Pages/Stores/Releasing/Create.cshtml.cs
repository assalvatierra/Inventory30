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
        public ReleasingCreateModel ReleasingCreateModel { get; set; }

        public IActionResult OnGet(int? storeId)
        {
            if (storeId == null)
            {
                return NotFound();
            }

            var storeList = storeServices.GetInvStores().ToList();

            ReleasingCreateModel = itemTrxServices.GetReleasingCreateModel_OnCreateOnGetAsync(InvTrxHdr, (int)storeId, GetUser(), storeList);

           // ViewData["InvStoreId"] = new SelectList(storeServices.GetInvStores(), "Id", "StoreName", storeId);
            //ViewData["InvTrxHdrStatusId"] = new SelectList(itemTrxServices.GetInvTrxHdrStatus(), "Id", "Status");
            //ViewData["InvTrxTypeId"] = new SelectList(itemTrxServices.GetInvTrxHdrTypes(), "Id", "Type", 2);
            //ViewData["UserId"] = User.FindFirstValue(ClaimTypes.Name);
            //ViewData["StoreId"] = storeId;

            return Page();
        }

        [BindProperty]
        public InvTrxHdr InvTrxHdr { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            InvTrxHdr = ReleasingCreateModel.InvTrxHdr;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            //itemTrxServices.CreateInvTrxHdrs(ReleasingCreateModel.InvTrxHdr);
            //await itemTrxServices.SaveChanges();
            await itemTrxServices.GetReleasingCreateModel_OnCreateOnPostAsync(InvTrxHdr);

            return RedirectToPage("./Details", new { id = InvTrxHdr.Id});
        }

        private string GetUser()
        {
            return User.FindFirstValue(ClaimTypes.Name);
        }
    }
}
