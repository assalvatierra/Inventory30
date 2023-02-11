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
           
            foreach (var item in ApprovedItemDetails)
            { 
                //get item Details
                var itemDetails = _context.InvItems
                    .Find(item.InvItemId);

                //check if item is in the search result list
                var IsItemInList = (ItemSearchResults.Where(c => c.Id == item.InvItemId).Count() == 0) && item.ItemQty > 0;

                //if item is not in the list
                if (IsItemInList)
                {

                    //adding item to the list
                    ItemSearchResults.Add(new ItemSearchResult
                    {
                        Id = item.InvItemId,
                        Item = itemDetails.Description,
                        Qty = services.GetAvailableCountByItem(item.InvItemId),
                        Code = itemDetails.Code,
                        ItemRemarks = itemDetails.Remarks,
                        Uom = itemDetails.InvUom.uom,
                        ItemSpec = GetItemCustomSpec(item.InvItemId)
                    });
                }
            }
            
            //apply search by input string of the existing list
            if (!String.IsNullOrEmpty(SearchStr))
            {
                var srchString = SearchStr.ToLower();
                ItemSearchResults = ItemSearchResults.Where(
                    c => c.Item.ToLower().Contains(srchString) ||
                         ( !String.IsNullOrEmpty(c.ItemRemarks) && c.ItemRemarks.ToLower().Contains(srchString) )
                    ).ToList();
            }

        }

        private string GetItemCustomSpec(int itemId)
        {

            //get item Details
            var itemSpecResult = _context.InvItemCustomSpecs
                .Where(i => i.InvItemId == itemId)
                .Include(i => i.InvCustomSpec)
                .ToList();

            string _itemSpec = "";
            if (itemSpecResult != null)
            {
                foreach (var spec in itemSpecResult)
                {
                    _itemSpec += spec.InvCustomSpec.SpecName + " : " 
                        + spec.SpecValue + " " + spec.InvCustomSpec.Measurement + " " 
                        + spec.Remarks + ", ";
                }
            }

            return _itemSpec;

        }

    }
}
