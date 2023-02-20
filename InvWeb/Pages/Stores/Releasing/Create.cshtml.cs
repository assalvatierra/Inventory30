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

        public IActionResult OnGet(int? storeId)
        {
            if (storeId == null)
            {
                return NotFound();
            }

            ViewData["InvStoreId"] = new SelectList(storeServices.GetInvStores(), "Id", "StoreName", storeId);
            ViewData["InvTrxHdrStatusId"] = new SelectList(itemTrxServices.GetInvTrxHdrStatus(), "Id", "Status");
            ViewData["InvTrxTypeId"] = new SelectList(itemTrxServices.GetInvTrxHdrTypes(), "Id", "Type", 2);
            ViewData["UserId"] = User.FindFirstValue(ClaimTypes.Name);
            ViewData["StoreId"] = storeId;

            return Page();
        }

        [BindProperty]
        public InvTrxHdr InvTrxHdr { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            itemTrxServices.CreateInvTrxHdrs(InvTrxHdr);
            await itemTrxServices.SaveChanges();

            //return RedirectToPage("./Index", new { storeId = InvTrxHdr.InvStoreId, status = "PENDING" });
            return RedirectToPage("./Details", new { id = InvTrxHdr.Id});
        }
    }
}
