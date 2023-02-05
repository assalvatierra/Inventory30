using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLib.Inventory.Models;
using CoreLib.Inventory.Models.Items;

namespace InvWeb.Data.Interfaces
{
    public interface IItemSpecServices
    {
        public Task<int> AddItemSpecification(InvItemSpec_Steel invItemSpec);
        public Task<int> EditItemSpecification(InvItemSpec_Steel invItemSpec);
        public Task<int> DeleteItemSpecification(InvItemSpec_Steel invItemSpec);
        public InvItemSpec_Steel GetItemSpecification(int id);
        public List<InvItemSpec_Steel> GetItemSpecification_ByInvItemId(int invItemId);
        public Task<bool> CheckItemHasAnyInvSpec(int invItemId);

        //public SelectList GetDefindSpecsSelectList();
        public IEnumerable<InvItemSysDefinedSpecs> GetDefinedSpecs();
        
        public bool IsCategoryHaveSpecDefs(int? categoryId);


        public Task<int> AddItemCustomSpecification(InvItemCustomSpec invItemSpec);
        public Task<int> EditItemCustomSpecification(InvItemCustomSpec invItemSpec);
        public Task<int> DeleteItemCustomSpecification(InvItemCustomSpec invItemSpec);
        public InvItemCustomSpec GetItemCustomSpecification(int id);
        public List<InvItemCustomSpec> GetItemCustomSpecification_ByInvItemId(int invItemId);

        public InvCustomSpec GetCustomSpecification(int id);
    }
}
