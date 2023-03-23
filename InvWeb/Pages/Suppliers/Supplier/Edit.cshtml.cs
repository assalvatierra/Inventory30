using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using CoreLib.DTO.Supplier;
using CoreLib.Interfaces;
using Inventory;
using Microsoft.Extensions.Logging;

namespace InvWeb.Suppliers.Supplier
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EditModel> _logger;
        private readonly ISupplierServices supplierServices;

        public EditModel(ApplicationDbContext context, ILogger<EditModel> logger)
        {
            _context = context;
            _logger = logger;
            supplierServices = new SupplierServices(_context, _logger);
        }

        [BindProperty]
        public SupplierCreateEditModel SupplierEditModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (SupplierEditModel == null)
            {
                SupplierEditModel = new SupplierCreateEditModel();
            }

            SupplierEditModel.InvSupplier = await supplierServices.GetInvSupplierByIdAsync((int)id);

            if (SupplierEditModel.InvSupplier == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            supplierServices.UpdateInvSupplier(SupplierEditModel.InvSupplier);

            try
            {
                await supplierServices.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!supplierServices.InvSupplierExists(SupplierEditModel.InvSupplier.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

    }
}
