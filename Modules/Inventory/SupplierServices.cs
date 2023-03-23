using CoreLib.DTO.Supplier;
using CoreLib.Interfaces;
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
    public class SupplierServices : ISupplierServices
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly DBMasterService dbMaster;

        public SupplierServices(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
            dbMaster = new DBMasterService(_context, _logger);

        }

        public void CreateInvSupplier(InvSupplier invSupplier)
        {
            throw new NotImplementedException();
        }

        public void DeleteInvSupplierById(int id)
        {
            throw new NotImplementedException();
        }

        public InvSupplier GetInvSupplierById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InvSupplier> GetInvSuppliers()
        {
            throw new NotImplementedException();
        }

        public async Task<SupplierIndexModel> GetSupplierIndexModelOnIndexGet(SupplierIndexModel supplierIndex)
        {
            try
            {
                if (supplierIndex == null)
                {
                    supplierIndex = new SupplierIndexModel();
                }

                supplierIndex.InvSupplier = await dbMaster.InvSupplierDb.GetInvSuppliersAsync();

                return supplierIndex;
            }
            catch (Exception ex )
            {
                return null;
            }
        }

        public void UpdateInvSupplierById(InvSupplier invSupplier)
        {
            throw new NotImplementedException();
        }
    }
}
