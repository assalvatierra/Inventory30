using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Interfaces.DBAccess
{
    public interface IInvSupplierDb
    {
        public IQueryable<InvSupplier> GetSuppliers();
        public Task<IList<InvSupplier>> GetInvSuppliersAsync();
        public IList<InvSupplier> GetInvSuppliers();
        public InvSupplier GetInvSupplierbyId(int id);
        public Task<InvSupplier?> FindInvSupplierByIdAsync(int id);
        public void CreateInvSupplier(InvSupplier invSupplier);
        public void EditInvSupplier(InvSupplier invSupplier);
        public void DeleteInvSupplier(InvSupplier invSupplier);
    }
}
