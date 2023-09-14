using CoreLib.DTO.Common.TrxDetails;
using CoreLib.DTO.Receiving;
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
        public ReleasingItemDtlsCreateEditModel GetReleasingItemTrxDtlsModel_OnCreateOnGet(InvTrxDtl invTrxDtl, int hdrId, int invItemId);
        public ReceivingItemDtlsCreateEditModel GeReceivingItemDtlsCreateModel_OnCreateOnGet(InvTrxDtl invTrxDtl, int hdrId, int id);
        public ReceivingItemDtlsCreateEditModel GeReceivingItemDtlsEditModel_OnEditOnGet(InvTrxDtl invTrxDtl);
        public Task<TrxDetailsItemDetailsModel> GetTrxDetailsModel_OnDetailsAsync(int id);
        public Task<TrxDetailsItemDeleteModel> GetTrxDetailsModel_OnDeleteAsync(int id);

        //Adjustment
        public TrxItemsCreateEditModel GeItemDtlsCreateModel_OnCreateOnGet(InvTrxDtl invTrxDtl, int hdrId);
        public TrxItemsCreateEditModel GeItemDtlsEditModel_OnEditOnGet(InvTrxDtl invTrxDtl);

        // Operators
        public IQueryable<InvTrxDtlOperator> GetInvTrxDtlOperators();
    }
}
