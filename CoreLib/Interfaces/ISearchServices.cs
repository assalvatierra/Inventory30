using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLib.Inventory.Models;
using CoreLib.Inventory.Models.Stores;

namespace InvWeb.Data.Interfaces
{
    interface ISearchServices
    {
        public int GetAvailableCountByItem(int id, int? storeId);
        public int GetAvailableCountByItem(int id);
        public Task<List<InvTrxDtl>> GetInvDetailsByIdAsync(int id);
    }
}
