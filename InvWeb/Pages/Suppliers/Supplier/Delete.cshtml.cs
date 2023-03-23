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
using Inventory;
using Microsoft.Extensions.Logging;
using CoreLib.DTO.Supplier;

namespace InvWeb.Suppliers.Supplier
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DeleteModel> _logger;
        private readonly ISupplierServices supplierServices;

        public DeleteModel(ApplicationDbContext context, ILogger<DeleteModel> logger)
        {
            _context = context;
            _logger = logger;
            supplierServices = new SupplierServices(_context, _logger);
        }

        [BindProperty]
        public SupplierDeleteModel InvSupplierDelete { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (InvSupplierDelete == null)
            {
                InvSupplierDelete = new SupplierDeleteModel();
            }

            InvSupplierDelete.InvSupplier = await supplierServices.GetInvSupplierByIdAsync((int)id);

            if (InvSupplierDelete.InvSupplier == null)
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

            InvSupplierDelete.InvSupplier = await supplierServices.FindInvSupplierByIdAsync((int)id);

            if (InvSupplierDelete.InvSupplier != null)
            {
                supplierServices.DeleteInvSupplier(InvSupplierDelete.InvSupplier);
                await supplierServices.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
