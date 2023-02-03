using System.Collections.Generic;
using CoreLib.Inventory.Models;
using CoreLib.Models.API;

namespace CoreLib.Inventory.Interfaces
{
    public interface IUomServices
    {

        public IEnumerable<InvUom> GetUomSelectList();
        public IEnumerable<InvUom> GetUomSelectListByItemId(int? itemId);
        public IEnumerable<UomsApiModel.ItemOumList> GetItemUomListByItemId(int? itemId);
        public int GetConverted_ItemCount_ByDefaultUom(int itemId);
    }
}
