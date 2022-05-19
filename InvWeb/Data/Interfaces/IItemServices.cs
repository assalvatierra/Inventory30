using System.Collections.Generic;
using System.Linq;
using WebDBSchema.Models;
using WebDBSchema.Models.Items;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InvWeb.Data.Interfaces
{
    public interface IItemServices
    {
        public List<ItemLotNoSelect> GetLotNotItemList(int itemid, int storeId);

        public IOrderedQueryable<InvItem> GetInvItemsOrderedByCategory();

        public SelectList GetInvItemsSelectList();
        public SelectList GetInvItemsSelectList(int selected);
    }
}
