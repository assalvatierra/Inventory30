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
        public IEnumerable<InvSupplier> GetInvSuppliers();
        public void CreateInvSupplier(InvSupplier invSupplier);
        public void DeleteInvSupplierById(int id);
        public void UpdateInvSupplierById(InvSupplier invSupplier);
        public SupplierIndexModel GetSupplierIndexModelOnIndexGet(SupplierIndexModel supplierIndex);
       
    }
}
