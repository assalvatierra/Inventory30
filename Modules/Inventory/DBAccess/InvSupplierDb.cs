using CoreLib.Interfaces.DBAccess;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DBAccess
{
    public class InvSupplierDb : IInvSupplierDb
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public InvSupplierDb(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public void CreateInvSupplier(InvSupplier invSupplier)
        {
            _context.InvSuppliers.Add(invSupplier);
        }

        public void DeleteInvSupplier(InvSupplier invSupplier)
        {
            _context.InvSuppliers.Remove(invSupplier);
        }

        public void EditInvSupplier(InvSupplier invSupplier)
        {
            _context.Attach(invSupplier).State = EntityState.Modified;
        }

        public async Task<InvSupplier?> FindInvSupplierByIdAsync(int id)
        {
            return await _context.InvSuppliers.FindAsync(id);
        }

        public InvSupplier GetInvSupplierbyId(int id)
        {
            return _context.InvSuppliers.Find(id);
        }

        public IList<InvSupplier> GetInvSuppliers()
        {
            return _context.InvSuppliers.ToList();
        }
        public async Task<IList<InvSupplier>> GetInvSuppliersAsync()
        {
            return await _context.InvSuppliers.ToListAsync();
        }

        public IQueryable<InvSupplier> GetSuppliers()
        {
            return _context.InvSuppliers;
        }
    }
}
