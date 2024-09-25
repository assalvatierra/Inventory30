using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Inventory.Models.Items;
using InvWeb.Data.Services;
using CoreLib.Inventory.Interfaces;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Masterfiles.ItemSearch
{
    public class SearchModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private ISearchServices services;

        public SearchModel(ApplicationDbContext context, ISearchServices iservices)
        {
            _context = context;
            services = iservices;
        }

        public List<ItemSearchResult> ItemSearchResults { get;set; }

        [BindProperty(SupportsGet = true)]
        public string SearchStr { get; set; }

        public async Task OnGetAsync()
        {
            ItemSearchResults = new List<ItemSearchResult>();

            //get all accepted items details
            var ApprovedItemDetails = await services.GetApprovedInvDetailsAsync();

            //get items not in records
            var AllItemsList = await _context.InvItems
                    .Include(i => i.InvUom)
                    .Include(i => i.InvItemSpec_Steel)
                        .ThenInclude(i => i.SteelMainCat)
                    .Include(i => i.InvItemSpec_Steel)
                        .ThenInclude(i => i.SteelSubCat)
                    .Include(i => i.InvItemSpec_Steel)
                        .ThenInclude(i => i.SteelMaterial)
                    .Include(i => i.InvItemSpec_Steel)
                        .ThenInclude(i => i.SteelMaterialGrade)
                    .Include(i => i.InvItemSpec_Steel)
                        .ThenInclude(i => i.SteelOrigin)
                    .Include(i => i.InvItemSpec_Steel)
                        .ThenInclude(i => i.SteelBrand)
                    .Include(i => i.InvItemCustomSpecs)
                        .ThenInclude(i => i.InvCustomSpec)
                    .ToListAsync();

            foreach (var item in AllItemsList)
            {
                //get item Details
                //var itemDetails = 
                //    .FirstOrDefaultAsync(m => m.Id == item.Id);

                //check if item is in the search result list
                var IsItemInList = (ItemSearchResults.Where(c => c.Id == item.Id).Count() == 0);

                //if item is not in the list
                if (IsItemInList)
                {
                    //adding item to the list
                    var newItemResult = new ItemSearchResult();
                    newItemResult.Id = item.Id;
                    newItemResult.Item = item.Description;
                    newItemResult.Qty = services.GetAvailableCountByItem(item.Id);
                    newItemResult.Code = item.Code;
                    newItemResult.ItemRemarks = item.Remarks;
                    newItemResult.Uom = item.InvUom.uom;
                    newItemResult.InvItemSpec_Steel = item.InvItemSpec_Steel.FirstOrDefault();
                    newItemResult.ItemMaster = GetItemMaster(item);

                    ItemSearchResults.Add(newItemResult);
                }
            }


            //apply search by input string of the existing list
            if (!String.IsNullOrEmpty(SearchStr))
            {
                var srchString = SearchStr.ToLower();
                ItemSearchResults = ItemSearchResults.Where(
                    c => c.Item.ToLower().Contains(srchString) ||
                         (!String.IsNullOrEmpty(c.ItemRemarks) && c.ItemRemarks.ToLower().Contains(srchString))
                    ).ToList();
            }



            // Get Items from SQL
            //var itemsOnStock = await services.GetItemsOnStock();

            //var checkItemOnStock = 0;
        }

        private string GetItemCustomSpec(ICollection<InvItemCustomSpec> invItemCustomSpec)
        {

         
            string _itemSpec = "";
            if (invItemCustomSpec != null)
            {
                foreach (var spec in invItemCustomSpec)
                {
                    _itemSpec += spec.InvCustomSpec.SpecName + " : " 
                        + spec.SpecValue + " " + spec.InvCustomSpec.Measurement + " " 
                        + spec.Remarks + ", ";
                }
            }

            return _itemSpec;

        }

        private InvItemSpec_Steel GetItemSpec_Steel(InvItem item)
        {
            if (item.InvItemSpec_Steel != null)
            {
                return item.InvItemSpec_Steel.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }


        private InvItemMaster GetItemMaster(InvItem item)
        {
            if (item != null)
            {
                return _context.InvItemMasters
                    .Include(c => c.InvItemBrand)
                    .Include(c => c.InvItemOrigin)
                    .Where(i=>i.InvItemId == item.Id).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }


    }
}
