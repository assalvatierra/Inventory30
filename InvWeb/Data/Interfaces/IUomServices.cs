using System.Collections.Generic;
using System.Linq;
using WebDBSchema.Models;
using WebDBSchema.Models.Items;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InvWeb.Data.Interfaces
{
    public interface IUomServices
    {

        public SelectList GetUomSelectList();
        public SelectList GetUomSelectListByItemId(int? itemId);
        public int GetConverted_ItemCount_ByDefaultUom(int itemId);
    }
}
