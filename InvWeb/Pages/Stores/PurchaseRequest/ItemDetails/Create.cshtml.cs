using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreLib.Inventory.Models;
using InvWeb.Data.Services;
using CoreLib.Inventory.Interfaces;
using CoreLib.Models.Inventory;
using Modules.Inventory;
using CoreLib.DTO.PurchaseOrder;
using CoreLib.Interfaces;
using Inventory;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace InvWeb.Pages.Stores.PurchaseRequest.ItemDetails
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;
        private readonly IInvPOItemServices invPOItemServices;


        public CreateModel(ApplicationDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
            invPOItemServices = new InvPOItemServices(_context, _logger);
        }

        [BindProperty]
        public InvPOItemCreateEditModel InvPOItemCreate { get; set; }
        public InvPoItem InvPoItem { get; set; }


        public IActionResult OnGet(int? hdrId)
        {
            if (hdrId == null)
            {
                return NotFound();
            }

            InvPOItemCreate = invPOItemServices.GetInvPOItemModel_OnCreate(InvPOItemCreate, (int)hdrId);

            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            invPOItemServices.CreateInvPoItem(InvPOItemCreate.InvPoItem);
            await invPOItemServices.SaveChangesAsync();

            return RedirectToPage("../Details", new { id = InvPOItemCreate.InvPoItem.InvPoHdrId });
        }
        private string GetUser()
        {
            return User.FindFirstValue(ClaimTypes.Name);
        }
    }
}
