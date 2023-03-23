using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Interfaces;
using Microsoft.Extensions.Logging;
using Modules.Inventory;
using CoreLib.Interfaces;
using Inventory;

namespace InvWeb.Suppliers.Supplier
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;
        private readonly ISupplierServices supplierServices;

        public CreateModel(ApplicationDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
            supplierServices = new SupplierServices(_context, _logger);
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public InvSupplier InvSupplier { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InvSuppliers.Add(InvSupplier);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
