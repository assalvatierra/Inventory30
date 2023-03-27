using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Interfaces.DBAccess
{
    public interface IInvSupItemDb
    {
        public IQueryable<InvSupplierItem> GetSupplierItems();
        public Task<IList<InvSupplierItem>> GetInvSupplierItemsAsync();
        public IList<InvSupplierItem> GetInvSupplierItems();
        public IList<InvSupplierItem> GetInvSupItemsBySupplierId(int supplierId);
        public InvSupplierItem GetInvSupplierbyId(int id);
        public Task<InvSupplierItem?> FindInvSupplierByIdAsync(int id);
        public void CreateInvSupplier(InvSupplierItem invSupplierItem);
        public void EditInvSupplier(InvSupplierItem invSupplierItem);
        public void DeleteInvSupplier(InvSupplierItem invSupplierItem);
    }
}
