using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLib.DTO.InvItems;
using CoreLib.Inventory.Models;
using CoreLib.Inventory.Models.Stores;
using CoreLib.Models.Inventory;

namespace CoreLib.Inventory.Interfaces
{
    public interface ISearchServices
    {
        public int GetAvailableCountByItem(int id, int? storeId);
        public int GetAvailableCountByItem(int id);
        public Task<IEnumerable<InvTrxDtl>> GetInvDetailsByIdAsync(int id);
        public Task<IEnumerable<InvTrxDtl>> GetApprovedInvDetailsAsync();

        public  Task<List<InvItemSearch>> GetItemsOnStock();
    }
}
