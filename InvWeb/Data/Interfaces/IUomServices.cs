using System.Collections.Generic;
using System.Linq;
using CoreLib.Inventory.Models;
using CoreLib.Inventory.Models.Items;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using InvWeb.Data.Models;
using Microsoft.Build.Framework;

namespace InvWeb.Data.Interfaces
{
    public interface IUomServices
    {

        public IEnumerable<InvUom> GetUomSelectList();
        public IEnumerable<InvUom> GetUomSelectListByItemId(int? itemId);
        public IEnumerable<InvUom> GetUomListByItemId(int? itemId);
        public Task<List<UomsApiModel.ItemOumList>> GetUomListByItemIdAsync(int? itemId);
        public int GetConverted_ItemCount_ByDefaultUom(int itemId);
    }
}
