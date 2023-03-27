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
    public class InvSupItemDb : IInvSupItemDb
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public InvSupItemDb(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }
        public void CreateInvSupplier(InvSupplierItem invSupplierItem)
        {
            _context.InvSupplierItems.Add(invSupplierItem);
        }

        public void DeleteInvSupplier(InvSupplierItem invSupplierItem)
        {
            _context.InvSupplierItems.Remove(invSupplierItem);
        }

        public void EditInvSupplier(InvSupplierItem invSupplierItem)
        {
            _context.Attach(invSupplierItem).State = EntityState.Modified;
        }

        public async Task<InvSupplierItem?> FindInvSupplierByIdAsync(int id)
        {
            return await _context.InvSupplierItems.FindAsync(id);
        }

        public IList<InvSupplierItem> GetInvSupItemsBySupplierId(int supplierId)
        {
            return _context.InvSupplierItems.Where(i => i.InvSupplierId == supplierId).ToList();
        }

        public InvSupplierItem GetInvSupplierbyId(int id)
        {
            return _context.InvSupplierItems.Find(id);
        }

        public IList<InvSupplierItem> GetInvSupplierItems()
        {
            return _context.InvSupplierItems.ToList();
        }

        public async Task<IList<InvSupplierItem>> GetInvSupplierItemsAsync()
        {
            return await _context.InvSupplierItems.ToListAsync();
        }

        public IQueryable<InvSupplierItem> GetSupplierItems()
        {
            return _context.InvSupplierItems;
        }
    }
}
