using System.Collections.Generic;
using System.Linq;
using CoreLib.Inventory.Models;
using CoreLib.Inventory.Models.Items;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InvWeb.Data.Interfaces
{
    public interface IItemServices
    {
        public List<ItemLotNoSelect> GetLotNotItemList(int itemid, int storeId);
        public IOrderedQueryable<InvItem> GetInvItemsOrderedByCategory();
        public SelectList GetInvItemsSelectList();
        public SelectList GetInvItemsSelectList(int selected);
        public SelectList GetInStockedInvItemsSelectList(List<int> storeItems);
        public SelectList GetInStockedInvItemsSelectList(int selected, List<int> storeItems);
        public SelectList GetConvertableUomSelectList();
    }
}
