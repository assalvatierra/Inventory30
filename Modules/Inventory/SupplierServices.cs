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
            try
            {

               dbMaster.InvSupplierDb.CreateInvSupplier(invSupplier);

            }
            catch (Exception ex)
            {

                _logger.LogError("SupplierServices:" + ex.Message);
            }
        }

        public void UpdateInvSupplier(InvSupplier invSupplier)
        {
            try
            {
                dbMaster.InvSupplierDb.EditInvSupplier(invSupplier);
            }
            catch (Exception ex)
            {
                _logger.LogError("SupplierServices:" + ex.Message);
            }
        }

        public bool InvSupplierExists(int id)
        {
            return _context.InvSuppliers.Any(e => e.Id == id);
        }

        public void DeleteInvSupplier(InvSupplier invSupplier)
        {
                
            try
            {
                dbMaster.InvSupplierDb.DeleteInvSupplier(invSupplier);
            }
            catch (Exception ex)
            {
                _logger.LogError("SupplierServices:" + ex.Message);
            }
        }


        public async Task<InvSupplier> FindInvSupplierByIdAsync(int id)
        {
            return await _context.InvSuppliers.FindAsync(id);

        }


        public async Task<InvSupplier> GetInvSupplierByIdAsync(int id)
        {
            return await _context.InvSuppliers.FirstOrDefaultAsync(m => m.Id == id);

        }

        public InvSupplier GetInvSupplierById(int id)
        {

            return _context.InvSuppliers.FirstOrDefault(m => m.Id == id);

        }

        public IEnumerable<InvSupplier> GetInvSuppliers()
        {
            return dbMaster.InvSupplierDb.GetInvSuppliers();
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
                _logger.LogError("SupplierServices:" + ex.Message);
                return null;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
