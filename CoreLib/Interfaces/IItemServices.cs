using CoreLib.Inventory.Models;
using CoreLib.Inventory.Models.Items;

namespace CoreLib.Inventory.Interfaces
{
    public interface IItemServices
    {
        public IEnumerable<ItemLotNoSelect> GetLotNotItemList(int itemid, int storeId);
        public IOrderedQueryable<InvItem> GetInvItemsOrderedByCategory();
        public IOrderedQueryable<InvItem> GetInvItemsSelectList();
        public IOrderedQueryable<InvItem> GetInvItemsSelectList(int selected);
        public IOrderedQueryable<InvItem> GetInStockedInvItemsSelectList(List<int> storeItems);
        public IOrderedQueryable<InvItem> GetInStockedInvItemsSelectList(int selected, List<int> storeItems);
        public IQueryable<InvUom> GetConvertableUomSelectList();
    }
}
