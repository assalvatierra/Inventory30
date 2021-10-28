using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvWeb.Data;
using WebDBSchema.Models;
using WebDBSchema.Models.Stores;

namespace InvWeb.Data.Interfaces
{
    interface IStoreServices
    {
        public Task<IEnumerable<StoreInvCount>> GetStoreItemsSummary(int storeId);
        public Task<List<InvTrxDtl>> GetReceivedItemsAsync(int storeId);
        public Task<List<InvTrxDtl>> GetReleasedItemsAsync(int storeId);
        public Task<List<InvTrxDtl>> GetAdjustmentItemsAsync(int storeId);
        public int GetAdjustmentItemsCount(List<InvTrxDtl> adjustmentItems, int itemId);
        public int GetAvailableItemsCount();

        public List<InvStore> GetStoreUsers(string user);
    }
}
