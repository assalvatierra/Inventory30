using CoreLib.DTO.Supplier;
using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Interfaces
{
    public interface ISupplierServices
    {

        public InvSupplier GetInvSupplierById(int id);
        public Task<InvSupplier> GetInvSupplierByIdAsync(int id);
        public Task<InvSupplier> FindInvSupplierByIdAsync(int id);
        public IEnumerable<InvSupplier> GetInvSuppliers();
        public void CreateInvSupplier(InvSupplier invSupplier);
        public void DeleteInvSupplier(InvSupplier invSupplier);
        public void UpdateInvSupplier(InvSupplier invSupplier);
        public Task<SupplierIndexModel> GetSupplierIndexModelOnIndexGet(SupplierIndexModel supplierIndex);
        public bool InvSupplierExists(int id);
        public Task SaveChangesAsync();
       
    }
}
