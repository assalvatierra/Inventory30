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
        private readonly ILogger<DeleteModel> _logger;
        private readonly IInvPOItemServices invPOItemServices;

        public DeleteModel(ApplicationDbContext context, ILogger<DeleteModel> logger)
        {
            _context = context;
            _logger = logger;
            invPOItemServices = new InvPOItemServices(_context, _logger);
        }

        [BindProperty]
        public InvPOItemDelete InvPOItemDelete { get; set; }
        public int InvHdrId { get; set; }

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

            InvHdrId = InvPOItemDelete.InvHdrId;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var InvPOItem = await invPOItemServices.GetInvPoItemById((int)id);
           

            if(InvPOItem == null)
            {
                return NotFound();
            }

            await invPOItemServices.DeleteInvPoItem((int)id);

            return RedirectToPage("../Details", new { id = InvPOItem.InvPoHdrId });
        }
    }
}
