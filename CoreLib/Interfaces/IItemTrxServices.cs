using CoreLib.Inventory.Models.Items;
using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Inventory.Interfaces
{
    public interface IItemTrxServices
    {
        public void CreateInvTrxHdrs();
        public void EditInvTrxHdrs(InvTrxHdr invTrxHdr);
        public void DeleteInvTrxHdrs();
        public Task SaveChanges();

        public IQueryable<InvTrxHdr> GetInvTrxHdrs();
        public IQueryable<InvTrxHdr> GetInvTrxHdrsById(int Id);
        public IQueryable<InvTrxHdr> GetTrxHdrsByStoreId_Releasing(int storeId);
        public IQueryable<InvTrxHdr> GetInvTrxHdrsByStoreId(int storeId, int typeId);

        //Services - Filters
        public IList<InvTrxHdr> FilterByStatus(IList<InvTrxHdr> invTrxHdrs, string status);
        public IList<InvTrxHdr> FilterByOrder(IList<InvTrxHdr> invTrxHdrs, string order);

        //TrxHeaders Status & Types
        public IQueryable<InvTrxHdrStatus> GetInvTrxHdrStatus();
        public IQueryable<InvTrxType> GetInvTrxHdrTypes();

    }
}
