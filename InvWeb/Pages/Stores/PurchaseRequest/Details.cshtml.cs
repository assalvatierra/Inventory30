using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using CoreLib.DTO.PurchaseOrder;
using CoreLib.Interfaces;
using Inventory;
using Microsoft.Extensions.Logging;
using DevExpress.Printing.Utils.DocumentStoring;

namespace InvWeb.Pages.Stores.PurchaseRequest
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IInvPOHdrServices invPOHdrServices;
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(ApplicationDbContext context, ILogger<DetailsModel> logger)
        {
            _context = context;
            _logger = logger;
            invPOHdrServices = new InvPOHdrServices(_context, _logger);
        }

        public InvPOHdrDetailsModel InvPoHdrDetails { get; set; }

        public int StoredID {get;set; }
        public string Status { get; set; }
        //public InvPoHdr InvPoHdr { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvPoHdrDetails = await invPOHdrServices.GetInvPOHdrModel_OnDetails(InvPoHdrDetails, (int)id, Status, IsUserAdminRole());

            if (InvPoHdrDetails.InvPoHdr == null)
            {
                return NotFound();
            }
            return Page();
        }
        private bool IsUserAdminRole()
        {
            return User.IsInRole("Admin");
        }
    }
}
