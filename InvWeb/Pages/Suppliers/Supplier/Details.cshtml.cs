using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using CoreLib.DTO.Supplier;
using CoreLib.Interfaces;
using Inventory;
using Microsoft.Extensions.Logging;

namespace InvWeb.Suppliers.Supplier
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DetailsModel> _logger;
        private readonly ISupplierServices supplierServices;

        public DetailsModel(ApplicationDbContext context, ILogger<DetailsModel> logger)
        {
            _context = context;
            _logger = logger;
            supplierServices = new SupplierServices(_context, _logger);
        }

        [BindProperty]
        public SupplierDetailsModel InvSupplierDetails { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (InvSupplierDetails == null)
            {
                InvSupplierDetails = new SupplierDetailsModel();
            }

            InvSupplierDetails.InvSupplier = await supplierServices.GetInvSupplierByIdAsync((int)id);

            if (InvSupplierDetails.InvSupplier == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
