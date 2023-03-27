using CoreLib.DTO.Supplier;
using CoreLib.DTO.SupplierItem;
using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib
{
    public interface ISupItemServices
    {

        public InvSupplierItem GetInvSupplierById(int id);
        public Task<InvSupplierItem> GetInvSupplierByIdAsync(int id);
        public Task<InvSupplierItem> FindInvSupplierByIdAsync(int id);
        public IEnumerable<InvSupplierItem> GetInvSuppliers();
        public void CreateInvSupplier(InvSupplierItem invSupplierItem);
        public void DeleteInvSupplier(InvSupplierItem invSupplierItem);
        public void UpdateInvSupplier(InvSupplierItem invSupplierItem);
        public Task<SupplierItemCreateEditModel> GetSupplierItemIndexModel_OnIndexGet(SupplierItemCreateEditModel supplierIndex);
        public bool InvSupplierExists(int id);
        public Task SaveChangesAsync();

    }
}
