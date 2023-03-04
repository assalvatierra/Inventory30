using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using InvWeb.Data;
using CoreLib.Inventory.Models;
using CoreLib.Inventory.Models.Stores;

namespace CoreLib.Inventory.Interfaces
{
    public interface IStoreServices
    {

        public IQueryable<InvStore> GetInvStores();
        public Task<IEnumerable<StoreInvCount>> GetStoreItemsSummary(int storeId, int categoryId, string sort);
        public Task<List<InvTrxDtl>> GetReceivedItemsAsync(int storeId);
        public Task<List<InvTrxDtl>> GetReleasedItemsAsync(int storeId);
        public Task<List<InvTrxDtl>> GetAdjustmentItemsAsync(int storeId);

        public int GetAdjustmentItemsCount(List<InvTrxDtl> adjustmentItems, int itemId);
        public List<int> GetAvailableItemsIdsByStore(int storeId);
        public List<InvStore> GetStoreUsers(string user);
        public string GetStoreName(int storeId);
        public decimal ConvertItemUomtoDefault(InvItem item, InvTrxDtl invTrxDtl, int itemCount);

        public Task<List<InvCategory>> GetCategoriesList();

        public Task<int> GetReceivingPendingAsync(int storeId);
        public Task<int> GetReleasingPendingAsync(int storeId);
        public Task<int> GetAdjustmentPendingAsync(int storeId);
        public Task<int> GetPurchaseOrderPendingAsync(int storeId);
        public Task<IEnumerable<InvTrxHdr>> GetRecentTransactions(int storeId);
        public Task<InvStore> GetStorebyIdAsync(int id);

    }
}
