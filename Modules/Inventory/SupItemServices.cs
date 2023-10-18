using CoreLib;
using CoreLib.DTO.SupplierItem;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using Inventory.DBAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory
{
    public class SupItemServices : ISupItemServices
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly DBMasterService dbMaster;

        public SupItemServices(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
            dbMaster = new DBMasterService(_context, _logger);

        }

        public void CreateInvSupplier(InvSupplierItem invSupplierItem)
        {
            throw new NotImplementedException();
        }

        public void DeleteInvSupplier(InvSupplierItem invSupplierItem)
        {
            throw new NotImplementedException();
        }

        public Task<InvSupplierItem> FindInvSupplierByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public InvSupplierItem GetInvSupplierById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<InvSupplierItem> GetInvSupplierByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InvSupplierItem> GetInvSuppliers()
        {
            throw new NotImplementedException();
        }

        public Task<SupplierItemCreateEditModel> GetSupplierItemIndexModel_OnCreateGet(SupplierItemCreateEditModel supplierIndex)
        {
            throw new NotImplementedException();
        }

        public Task<SupplierItemCreateEditModel> GetSupplierItemIndexModel_OnIndexGet(SupplierItemCreateEditModel supplierIndex)
        {
            throw new NotImplementedException();
        }

        public async Task<SupplierItemIndexModel> GetSupplierItemIndexModel_OnIndexGet(SupplierItemIndexModel supplierIndex)
        {

            if (_context.InvSuppliers.Find(supplierIndex.SupplierId) != null)
            {

                var supplier = _context.InvSuppliers.Find(supplierIndex.SupplierId);

                var supItems = await _context.InvSupplierItems.Where(s => s.InvSupplierId == supplierIndex.SupplierId).ToListAsync();


                var supItems_steel = await _context.InvItemSpec_Steel
                    .Include(s=>s.SteelBrand)
                    .Include(s => s.SteelMaterial)
                    .Include(s => s.SteelMainCat)
                    .Include(s => s.SteelMaterialGrade)
                    .Include(s => s.SteelOrigin)
                    .Include(s => s.SteelSubCat)
                    .Include(s => s.SteelSize)
                    .Include(s => s.InvItem)
                    .Where(s => s.SteelBrand.Name == supplier.Name).ToListAsync();


                return new SupplierItemIndexModel
                {
                    SupplierId = supplierIndex.SupplierId,
                    InvSupplierItem = supItems,
                    InvItemSpec_Steels = supItems_steel
                };

            } 

            return new SupplierItemIndexModel
            {
                SupplierId = supplierIndex.SupplierId,
                InvSupplierItem = new List<InvSupplierItem> { },
                InvItemSpec_Steels = new List<InvItemSpec_Steel> { }
            };
        }

        public bool InvSupplierExists(int id)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public void UpdateInvSupplier(InvSupplierItem invSupplierItem)
        {
            throw new NotImplementedException();
        }
    }
}
