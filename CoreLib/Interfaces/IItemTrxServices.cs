using CoreLib.Inventory.Models.Items;
using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLib.DTO.Releasing;

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

        //Get View Models Data
        public Task<ReleasingIndexModel> GetReleasingIndexModel_OnIndexOnGetAsync(IList<InvTrxHdr> invTrxHdrs, int storeId, int TypeId, string status, bool userIsAdmin);
        public Task<ReleasingIndexModel> GetReleasingIndexModel_OnIndexOnPostAsync(IList<InvTrxHdr> invTrxHdrs, int storeId, int TypeId, string status, string orderBy, bool userIsAdmin);
        public ReleasingCreateEditModel GetReleasingCreateModel_OnCreateOnGet(InvTrxHdr invTrxHdr, int storeId, string User, IList<InvStore> invStoreList);
        public ReleasingCreateEditModel GetReleasingEditModel_OnEditOnGet(InvTrxHdr invTrxHdr, int storeId, string User, IList<InvStore> invStoreList);
        public Task DeleteInvTrxHdrs_AndTrxDtlsItems(InvTrxHdr invTrxHdr);

        //Transaction Details
        public IEnumerable<InvTrxDtl> GetInvTrxDtlsById(int Id);
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
