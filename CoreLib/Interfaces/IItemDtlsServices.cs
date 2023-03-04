using CoreLib.DTO.Releasing;
using CoreLib.Inventory.Models;
using CoreLib.Inventory.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Inventory.Interfaces
{
    public interface IItemDtlsServices
    {
        public void CreateInvDtls(InvTrxDtl invTrxDtl);
        public void EditInvDtls(InvTrxDtl invTrxDtl);
        public void DeleteInvDtls(InvTrxDtl invTrxDtl);
        public Task SaveChangesAsync();
        public bool InvTrxDtlsExists(int id);

        public IQueryable<InvTrxDtl> GetInvDtlsById(int Id);
        public Task<InvTrxDtl> GetInvDtlsByIdAsync(int Id);
        public IQueryable<InvTrxDtl> GetInvDtlsByStoreId(int storeId, int typeId);
        public Task<InvTrxDtl> GetInvDtlsByIdOnEdit(int Id);

        //Create / Edit Models
        public ReleasingCreateEditModel GetReleasingItemTrxDtlsModel_OnCreateOnGet(InvTrxDtl invTrxDtl, int storeId, List<InvTrxHdr> invTrxHdrs, IList<InvItem> invItems, IList<ItemLotNoSelect> itemLotNoSelects, IList<int> availableItems, IList<InvUom> invUoms);
        //public ReleasingCreateEditModel GetReleasingEditModel_OnEditOnGet(InvTrxDtl invTrxDtl, int storeId, IList<InvStore> invStoreList);

        // Operators
        public IQueryable<InvTrxDtlOperator> GetInvTrxDtlOperators();
    }
}
