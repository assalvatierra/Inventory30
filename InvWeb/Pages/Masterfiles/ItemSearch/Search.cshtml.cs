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
using DevExpress.Data.ODataLinq.Helpers;
using Inventory.DBAccess;
using DevExpress.Printing.Utils.DocumentStoring;

namespace InvWeb.Pages.Masterfiles.ItemSearch
{
    public class SearchModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private ISearchServices services;


        protected readonly int TYPE_RECEIVED = 1;
        protected readonly int TYPE_RELEASED = 2;
        protected readonly int TYPE_ADJUSTMENT = 3;

        protected readonly int STATUS_REQUEST = 1;
        protected readonly int STATUS_APPROVED = 2;
        protected readonly int STATUS_CLOSED = 3;
        protected readonly int STATUS_CANCELLED = 4;

        protected readonly int OPERATOR_ADD = 1;
        protected readonly int OPERATOR_SUBTRACT = 2;

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
            //var ApprovedItemDetails = await services.GetApprovedInvDetailsAsync();

            List<InvItem> AllItemsList = await GetInvItemsSearch(SearchStr);
         
            List<InvStore> Stores = _context.InvStores.ToList();

            foreach (var store in Stores)
            {
                foreach (var item in AllItemsList)
                {
                    //check if item is in the search result list
                    //var IsItemInList = (ItemSearchResults.Where(c => c.Id == item.Id).Count() == 0);

                    //if item is not in the list
                    //if (IsItemInList)
                    //{
                    //adding item to the list
                    var newItemResult = new ItemSearchResult();
                        newItemResult.Id = item.Id;
                        newItemResult.Item = item.Description;
                        newItemResult.Code = item.Code;
                        newItemResult.ItemRemarks = item.Remarks;
                        newItemResult.Uom = item.InvUom.uom;
                        newItemResult.InvItemSpec_Steel = item.InvItemSpec_Steel.FirstOrDefault();
                        newItemResult.ItemMaster = GetItemMaster(item);

                        List<InvTrxDtl> invTrxDtls = GetItemInvDetailLists(item.Id, store.Id);

                        if (invTrxDtls.Count() > 0)
                        {
                            newItemResult.Qty = GetItemOnStock(invTrxDtls);
                            newItemResult.Qty_Requested = GetOnRequestCount(invTrxDtls);
                            newItemResult.Qty_Approved = GetOnApprovedCount(invTrxDtls);
                            newItemResult.Qty_Available = newItemResult.Qty - (newItemResult.Qty_Requested + newItemResult.Qty_Approved);
                            newItemResult.StoreId = store.Id;
                            newItemResult.InvStore = store.StoreName;
                            ItemSearchResults.Add(newItemResult);
                        }

                    //}
                }
            }

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

        private async  Task<List<InvItem>> GetInvItemsSearch(string SearchStr)
        {
            List<InvItem> AllItemsList = new List<InvItem>();

            //get items not in records
            if (string.IsNullOrEmpty(SearchStr))
            {
                //get items not in records
                AllItemsList = await _context.InvItems
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
            }
            else
            {
                var srchString = SearchStr.ToLower();
                AllItemsList = await _context.InvItems
                       .Where(c => c.Description.ToLower().Contains(srchString) ||
                         (!String.IsNullOrEmpty(c.Remarks) && c.Remarks.ToLower().Contains(srchString) ||
                         c.Code.Contains(srchString)))
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
            }

            return AllItemsList;
        }



        private int GetItemOnStock(List<InvTrxDtl> invTrxDtls)
        {
            var totalAvailableQty = 0;

            var itemDtls = invTrxDtls
                .Where(i =>i.InvTrxHdr.InvTrxHdrStatusId == STATUS_APPROVED  || i.InvTrxHdr.InvTrxHdrStatusId == STATUS_CLOSED)
                .ToList();

            var itemDtls_Closed = invTrxDtls
                .Where(i => i.InvTrxHdr.InvTrxHdrStatusId == STATUS_CLOSED)
                .ToList();

            var itemQtyAdd = itemDtls.Where(c => c.InvTrxDtlOperatorId == OPERATOR_ADD).Sum(c => c.ItemQty);
            var itemQtySub = itemDtls_Closed.Where(c => c.InvTrxDtlOperatorId == OPERATOR_SUBTRACT).Sum(c => c.ItemQty);

            totalAvailableQty = itemQtyAdd - itemQtySub;

            return totalAvailableQty;
        }

        private int GetOnRequestCount(List<InvTrxDtl> invTrxDtls)
        {
            var totalOnRequestQty = 0;
            var itemDtls = invTrxDtls
                .Where(i => i.InvTrxHdr.InvTrxHdrStatusId == STATUS_REQUEST)
                .ToList();

            if (itemDtls == null)
            {
                return 0;
            }

            var itemQtyAdd = itemDtls.Where(c => c.InvTrxDtlOperatorId == OPERATOR_ADD).Sum(c => c.ItemQty);
            var itemQtySub = itemDtls.Where(c => c.InvTrxDtlOperatorId == OPERATOR_SUBTRACT).Sum(c => c.ItemQty);

            totalOnRequestQty = itemQtySub;

            return totalOnRequestQty;

        }


        public int GetOnApprovedCount(List<InvTrxDtl> invTrxDtls)
        {
                var totalApprovedQty = 0;

                var itemDtls = invTrxDtls
                    .Where(i =>i.InvTrxHdr.InvTrxHdrStatusId == STATUS_APPROVED)
                    .ToList();

                if (itemDtls == null)
                {
                    return 0;
                }

                var itemQtyAdd = itemDtls.Where(c => c.InvTrxDtlOperatorId == OPERATOR_ADD).Sum(c => c.ItemQty);
                var itemQtySub = itemDtls.Where(c => c.InvTrxDtlOperatorId == OPERATOR_SUBTRACT).Sum(c => c.ItemQty);

                totalApprovedQty = itemQtySub;

                return totalApprovedQty;
        }



        private List<InvTrxDtl> GetItemInvDetailLists(int id, int? storeId)
        {

            if (storeId == 0 || id == 0)
            {
                return new List<InvTrxDtl>();
            }

            var itemDtls = _context.InvTrxDtls
                .Where(i => i.InvItemId == id
                    && (i.InvTrxHdr.InvTrxHdrStatusId == STATUS_REQUEST || i.InvTrxHdr.InvTrxHdrStatusId == STATUS_APPROVED || i.InvTrxHdr.InvTrxHdrStatusId == STATUS_CLOSED)
                    && i.InvTrxHdr.InvStoreId == storeId)
                .Include(i => i.InvTrxHdr)
                .ToList();

            return itemDtls;
        }



    }
}
