using System.Collections.Generic;
using System.Linq;
using WebDBSchema.Models;
using WebDBSchema.Models.Items;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using InvWeb.Data.Models;

namespace InvWeb.Data.Interfaces
{
    public interface IUomServices
    {

        public SelectList GetUomSelectList();
        public SelectList GetUomSelectListByItemId(int? itemId);
        public List<InvUom> GetUomListByItemId(int? itemId);
        public Task<List<UomsApiModel.ItemOumList>> GetUomListByItemIdAsync(int? itemId);
        public int GetConverted_ItemCount_ByDefaultUom(int itemId);
    }
}
