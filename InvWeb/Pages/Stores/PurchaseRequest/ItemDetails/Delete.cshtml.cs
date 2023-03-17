using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using CoreLib.Interfaces;
using Microsoft.Extensions.Logging;
using Inventory;
using CoreLib.DTO.PurchaseOrder;

namespace InvWeb.Pages.Stores.PurchaseRequest.ItemDetails
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;
        private readonly IInvPOItemServices invPOItemServices;

        public DeleteModel(ApplicationDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
            invPOItemServices = new InvPOItemServices(_context, _logger);
        }

        [BindProperty]
        public InvPOItemDelete InvPOItemDelete { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvPOItemDelete = await invPOItemServices.GetInvPOItemModel_OnDelete(InvPOItemDelete, (int)id);

            if (InvPOItemDelete.InvPoItem == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await invPOItemServices.DeleteInvPoItem((int)id);

            return RedirectToPage("../Details", new { id = InvPOItemDelete.InvPoItem.InvPoHdrId });
        }
    }
}
