using System.Collections.Generic;
using System.Linq;
using WebDBSchema.Models;
using WebDBSchema.Models.Items;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using InvWeb.Data.Models;


namespace InvWeb.Data.Interfaces
{
    public interface IItemSpecServices
    {
        public Task<int> AddItemSpecification(InvItemSpec_Steel invItemSpec);
        public Task<int> EditItemSpecification(InvItemSpec_Steel invItemSpec);
        public Task<int> DeleteItemSpecification(InvItemSpec_Steel invItemSpec);
        public InvItemSpec_Steel GetItemSpecification(int id);
        public List<InvItemSpec_Steel> GetItemSpecificationByInvItemId(int invItemId);
        public Task<bool> CheckItemHasAnyInvSpec(int invItemId);
        public SelectList GetDefindSpecsSelectList();
        public bool IsCategoryHaveSpecDefs(int? categoryId);
    }
}
