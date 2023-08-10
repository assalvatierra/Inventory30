using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using CoreLib;
using Inventory;
using Microsoft.Extensions.Logging;
using CoreLib.DTO.SupplierItem;

namespace InvWeb.Suppliers.SupplierItems
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;
        private readonly ISupItemServices supItemServices;

        public IndexModel(ApplicationDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
            supItemServices = new SupItemServices(_context, _logger);
        }

        public SupplierItemIndexModel SupplierItemIndex { get; set; }
        //public IList<InvSupplierItem> InvSupplierItem { get;set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SupplierItemIndex = new SupplierItemIndexModel();
            SupplierItemIndex.SupplierId = (int)id;

            SupplierItemIndex = await supItemServices.GetSupplierItemIndexModel_OnIndexGet(SupplierItemIndex);

            //InvSupplierItem = await _context.InvSupplierItems
            //    .Where(i => i.InvSupplierId == id)
            //    .Include(i => i.InvItem) 
            //    .Include(i => i.InvSupplier).ToListAsync();

            ViewData["Supplier"] = _context.InvSuppliers.Find(id).Name;
            ViewData["SupplierId"] = id;
            return Page();
        }
    }
}
