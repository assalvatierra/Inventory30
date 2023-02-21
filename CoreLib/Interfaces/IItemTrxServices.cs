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
        public void CreateInvTrxHdrs(InvTrxHdr invTrxHdr);
        public void EditInvTrxHdrs(InvTrxHdr invTrxHdr);
        public void DeleteInvTrxHdrs(InvTrxHdr invTrxHdr);
        public Task SaveChanges();
        public bool InvTrxHdrExists(int id);

        //Transaction Headers
        public IQueryable<InvTrxHdr> GetInvTrxHdrs();
        public IQueryable<InvTrxHdr> GetInvTrxHdrsById(int Id);
        public Task<InvTrxHdr> GetInvTrxHdrsByIdAsync(int Id);
        public IQueryable<InvTrxHdr> GetTrxHdrsByStoreId_Releasing(int storeId);
        public IQueryable<InvTrxHdr> GetInvTrxHdrsByStoreId(int storeId, int typeId);
        public int GetInvTrxStoreId(int hdrId);

        //Transaction Details
        public IQueryable<InvTrxDtl> GetInvTrxDtlsById(int Id);
        public IQueryable<InvTrxDtl> GetInvTrxDtlsByStoreId(int storeId, int typeId);
        public Task RemoveTrxDtlsList(int invHdrId);

        //Services - Filters
        public IList<InvTrxHdr> FilterByStatus(IList<InvTrxHdr> invTrxHdrs, string status);
        public IList<InvTrxHdr> FilterByOrder(IList<InvTrxHdr> invTrxHdrs, string order);

        //TrxHeaders Status & Types
        public IQueryable<InvTrxHdrStatus> GetInvTrxHdrStatus();
        public IQueryable<InvTrxType> GetInvTrxHdrTypes();

    }
}
