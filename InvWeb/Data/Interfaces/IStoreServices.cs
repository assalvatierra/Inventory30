using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvWeb.Data;
using CoreLib.Inventory.Models;
using CoreLib.Inventory.Models.Stores;

namespace InvWeb.Data.Interfaces
{
    interface IStoreServices
    {
        public Task<IEnumerable<StoreInvCount>> GetStoreItemsSummary(int storeId, int categoryId, string sort);
        public Task<List<InvTrxDtl>> GetReceivedItemsAsync(int storeId);
        public Task<List<InvTrxDtl>> GetReleasedItemsAsync(int storeId);
        public Task<List<InvTrxDtl>> GetAdjustmentItemsAsync(int storeId);
        public int GetAdjustmentItemsCount(List<InvTrxDtl> adjustmentItems, int itemId);
        public int GetAvailableItemsCountByStore();
        public List<InvStore> GetStoreUsers(string user);
        public string GetStoreName(int storeId);
        public decimal ConvertItemUomtoDefault(InvItem item, InvTrxDtl invTrxDtl, int itemCount);

        public Task<List<InvCategory>> GetCategoriesList();

    }
}
