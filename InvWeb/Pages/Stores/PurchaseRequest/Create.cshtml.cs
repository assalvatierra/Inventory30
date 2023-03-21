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
using Microsoft.Extensions.Logging;
using CoreLib.Interfaces;
using Inventory;
using CoreLib.DTO.PurchaseOrder;

namespace InvWeb.Pages.Stores.PurchaseRequest
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IInvPOHdrServices invPOHdrServices;
        private readonly ILogger<IndexModel> _logger;

        public CreateModel(ApplicationDbContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _context = context;
            invPOHdrServices = new InvPOHdrServices(_context, _logger);
        }
        [BindProperty]
        public InvPOHdrCreateEditModel InvPOHdrCreateModel { get; set; }

        public IActionResult OnGet(int? storeId)
        {
            if (storeId == null)
            {
                return NotFound();
            }

            InvPOHdrCreateModel = invPOHdrServices.GetInvPOHdrModel_OnCreate(InvPOHdrCreateModel, (int)storeId, GetUser());

            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            invPOHdrServices.CreateInvPoHdrs(InvPOHdrCreateModel.InvPoHdr);
            await invPOHdrServices.SaveChangesAsync();

            return RedirectToPage("./Index", new { storeId = InvPOHdrCreateModel.InvPoHdr.InvStoreId });
        }

        private string GetUser()
        {
            return User.FindFirstValue(ClaimTypes.Name);
        }
    }
}
