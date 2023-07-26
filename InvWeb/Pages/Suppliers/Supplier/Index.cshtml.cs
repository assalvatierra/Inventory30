using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using Microsoft.AspNetCore.Authorization;
using CoreLib.Models.Inventory;
using CoreLib.Interfaces;
using Microsoft.Extensions.Logging;
using Inventory;
using CoreLib.DTO.Supplier;

namespace InvWeb.Suppliers.Supplier
{
    [Authorize(Roles = "ADMIN")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;
        private readonly ISupplierServices supplierServices;

        public IndexModel(ApplicationDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
            supplierServices = new SupplierServices(_context, _logger);
        }


        public SupplierIndexModel SupplierIndexModel { get; set; }

        public async Task OnGetAsync()
        {
            SupplierIndexModel = await supplierServices.GetSupplierIndexModelOnIndexGet(SupplierIndexModel);
        }
    }
}
