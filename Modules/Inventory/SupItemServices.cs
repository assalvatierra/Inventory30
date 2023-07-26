using CoreLib;
using CoreLib.DTO.SupplierItem;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using Inventory.DBAccess;
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

        public Task<SupplierItemIndexModel> GetSupplierItemIndexModel_OnIndexGet(SupplierItemIndexModel supplierIndex)
        {
            throw new NotImplementedException();
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
