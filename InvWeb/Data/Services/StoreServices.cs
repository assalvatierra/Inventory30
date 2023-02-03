using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvWeb.Data.Interfaces;
using InvWeb.Data;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Inventory.Models.Stores;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Reflection.Metadata;
using CoreLib.Inventory.Interfaces;

namespace InvWeb.Data.Services
{
    public class StoreServices : IStoreServices
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;


        private readonly int TYPE_RECEIVED = 1;
        private readonly int TYPE_RELEASED = 2;
        private readonly int TYPE_ADJUSTMENT = 3;

        public StoreServices(ApplicationDbContext context)
        {
            _context = context;

        }

        public StoreServices(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;

        }

        public async Task<IEnumerable<StoreInvCount>> GetStoreItemsSummary(int storeId, int _categoryId, string sort)
        {
            try
            {

                decimal accepted = 0;
                decimal pending = 0;
                decimal released = 0;
                decimal requested = 0;

                List<InvTrxDtl> Received;
                List<InvTrxDtl> Released;
                List<InvTrxDtl> Adjustment;

                var invItems = await GetItemsAsync();

                //Todo: add filter to add only trx with approved status (statusId = 1) 
                if (_categoryId > 0)
                {
                     Received = await this.GetReleasedItemsByCatAsync(storeId, _categoryId);
                     Released = await this.GetReceivedItemsByCatAsync(storeId, _categoryId);
                     Adjustment = await this.GetAdjustmentItemsByCatAsync(storeId, _categoryId);
                }
                else
                {
                     Received = await this.GetReceivedItemsAsync(storeId);
                     Released = await this.GetReleasedItemsAsync(storeId);
                     Adjustment = await this.GetAdjustmentItemsAsync(storeId);
                }

                List<StoreInvCount> storeInvItems = new();

                foreach (var item in invItems)
                {
                    var itemDetails = invItems.Where(i => i.Id == item.Id).FirstOrDefault();
                    decimal itemReceived = ConvertItemsListUoms(Received.Where(h => h.InvItemId == item.Id).ToList());
                    decimal itemReleased = ConvertItemsListUoms(Released.Where(h => h.InvItemId == item.Id).ToList());

                    var itemCategoryDetails = await GetCategoryById(item.InvCategoryId);
                    string category = itemCategoryDetails != null ? itemCategoryDetails.Description : "NA";
                    int categoryId = itemCategoryDetails != null ? itemCategoryDetails.Id : 0;

                    if (Received != null)
                    {
                        accepted = GetAcceptedItemsCount(Received, item.Id);
                        pending = GetPendingItemsCount(Received, item.Id);
                    }

                    if (Released != null)
                    {
                        released = GetReleasedItemsCount(Released, item.Id);
                        requested = GetRequestedItemsCount(Released, item.Id);
                    }

                    int itemAdjustment = GetAdjustmentItemsCount(Adjustment, item.Id);


                    if (Received.Where(h => h.InvItemId == item.Id).Any())
                    {
                        storeInvItems.Add(AddItemToStoreInvcount(item.Id, itemDetails, itemReceived, itemReleased, itemAdjustment,
                            accepted, released, pending, requested, category, categoryId));
                    }

                    if (Released.Where(h => h.InvItemId == item.Id).Any() && !Received.Where(h => h.InvItemId == item.Id).Any())
                    {
                        storeInvItems.Add(AddItemToStoreInvcount(item.Id, itemDetails, itemReceived, itemReleased, itemAdjustment,
                            accepted, released, pending, requested, category, categoryId));
                    }

                }

                //sort items
                storeInvItems = SortInvCountItems(storeInvItems, sort);

                _logger.LogInformation("StoreServices : GetStoreItemsSummary ");

                return storeInvItems;

            }
            catch
            {
                _logger.LogError("StoreServices: Unable to GetStoreItemsSummary");
                throw new Exception("StoreServices: Unable to GetStoreItemsSummary.");
            }
        }

        private List<StoreInvCount> SortInvCountItems(List<StoreInvCount> _storeInvItems, String sort)
        {

            switch (sort)
            {
                case "ITEM":
                    return _storeInvItems.OrderBy(i => i.Item).ToList();
                case "CODE":
                    return _storeInvItems.OrderBy(i => i.Code).ToList();
                case "COUNT":
                    return _storeInvItems.OrderBy(i => i.OnHand).ToList();
                case "CATEGORY":
                    return _storeInvItems.OrderBy(i => i.Description).ToList();
                default:
                    return _storeInvItems;
            }

        }


        private StoreInvCount AddItemToStoreInvcount(int id, InvItem itemDetails, decimal itemReceived,
            decimal itemReleased, decimal itemAdjustment, decimal accepted, decimal released, decimal pending, decimal requested
            , string category, int categoryId)
        {

            //conversions

            StoreInvCount storeInv = new StoreInvCount();
            storeInv.Id = id;
            storeInv.Description = "(" + itemDetails.Code + ") " + itemDetails.Description + " " + itemDetails.Remarks;
            storeInv.Available = (itemReceived - itemReleased) + (itemAdjustment);
            storeInv.OnHand = (accepted - released) + (itemAdjustment);
            storeInv.ReceivePending = pending;
            storeInv.ReceiveAccepted = accepted;
            storeInv.ReleaseRequest = requested;
            storeInv.ReleaseReleased = released;
            storeInv.Adjustments = itemAdjustment;
            storeInv.InvWarningLevels = itemDetails.InvWarningLevels;
            storeInv.Category = category;
            storeInv.CategoryId = categoryId;
            storeInv.ItemSpec = GetItemCustomSpec(itemDetails.Id);
            storeInv.Uom = itemDetails.InvUom.uom;
            storeInv.Item = itemDetails.Description;
            storeInv.Code = itemDetails.Code;

            return storeInv;
        }

        private IOrderedEnumerable<StoreInvCount> GetInvCountOrderBy(List<StoreInvCount> storeInvItems, string orderBy)
        {
            if (orderBy != null)
            {

                switch (orderBy.ToLower())
                {
                    case "category":
                        return storeInvItems.OrderBy(i => i.CategoryId);
                    case "name":
                        return storeInvItems.OrderBy(i => i.Description);
                    case "count":
                        return storeInvItems.OrderBy(i => i.OnHand);
                }

                return storeInvItems.OrderBy(i => i.OnHand);

            }
            else
            {
                return storeInvItems.OrderBy(i => i.CategoryId);
            }

        }

        public int GetAdjustmentItemsCount(List<InvTrxDtl> adjustmentItems, int itemId)
        {
            try
            {
                int itemAdjustmentCount = 0;
                var itemAdjutmentList = adjustmentItems.Where(h => h.InvItemId == itemId);
                foreach (var adjustment in itemAdjutmentList)
                {
                    if (adjustment.InvTrxDtlOperatorId == 2)
                    {
                        itemAdjustmentCount -= (adjustment.ItemQty);
                    }
                    else
                    {
                        itemAdjustmentCount += (adjustment.ItemQty);
                    }
                }

                return itemAdjustmentCount;
            }
            catch
            {
                throw new Exception("StoreServices: Unable to GetAdjustmentItemsCount.");
            }
        }

        public int GetAvailableItemsCountByStore()
        {
            throw new NotImplementedException();
        }

        public int GetOnHandItemsCountByStore()
        {
            throw new NotImplementedException();
        }

        public decimal GetPendingItemsCount(List<InvTrxDtl> receivedItems, int itemId)
        {
            var pendingList = receivedItems.Where(h => h.InvTrxHdr.InvTrxHdrStatusId == 1 &&
                                            h.InvItemId == itemId).ToList();

            return ConvertItemsListUoms(pendingList);

        }

        public decimal GetAcceptedItemsCount(List<InvTrxDtl> receivedItems, int itemId)
        {
            var acceptedList = receivedItems.Where(h => h.InvTrxHdr.InvTrxHdrStatusId > 1 &&
                                            h.InvItemId == itemId).ToList();
            return ConvertItemsListUoms(acceptedList);
        }

        public decimal GetRequestedItemsCount(List<InvTrxDtl> releasedItems, int itemId)
        {
            var requestedList = releasedItems.Where(h => h.InvTrxHdr.InvTrxHdrStatusId == 1 &&
                                            h.InvItemId == itemId).ToList();
            return ConvertItemsListUoms(requestedList);
        }

        public decimal GetReleasedItemsCount(List<InvTrxDtl> releasedItems, int itemId)
        {
            var releasedList = releasedItems.Where(h => h.InvTrxHdr.InvTrxHdrStatusId > 1 &&
                                            h.InvItemId == itemId).ToList();
            return ConvertItemsListUoms(releasedList);
        }


        public async Task<int> GetReceivingPendingAsync(int storeId)
        {
            try
            {
                var storePendingReceiving = await _context.InvTrxHdrs
                    .Where(t => t.InvStoreId == storeId && t.InvTrxTypeId == 1 && t.InvTrxHdrStatusId == 1)
                    .ToListAsync();
                return storePendingReceiving.Count();
            }
            catch
            {
                throw new Exception("StoreServices: Unable to GetReceivingPendingAsync.");
            }
        }

        public async Task<int> GetReleasingPendingAsync(int storeId)
        {
            try {
                var storePendingReceiving = await _context.InvTrxHdrs
                    .Where(t => t.InvStoreId == storeId && t.InvTrxTypeId == 2 && t.InvTrxHdrStatusId == 1)
                    .ToListAsync();
                return storePendingReceiving.Count();
            }
            catch
            {
                throw new Exception("StoreServices: Unable to GetReleasingPendingAsync.");
            }
        }

        public async Task<int> GetAdjustmentPendingAsync(int storeId)
        {
            try {
                var storePendingReceiving = await _context.InvTrxHdrs
                    .Where(t => t.InvStoreId == storeId && t.InvTrxTypeId == 3 && t.InvTrxHdrStatusId == 1)
                    .ToListAsync();
                return storePendingReceiving.Count();
            }
            catch
            {
                throw new Exception("StoreServices: Unable to GetAdjustmentPendingAsync.");
            }
        }

        public async Task<int> GetPurchaseOrderPendingAsync(int storeId)
        {
            try
            {
                var storePendingReceiving = await _context.InvPoHdrs
                    .Where(t => t.InvStoreId == storeId && t.InvPoHdrStatusId == 1)
                    .ToListAsync();
                return storePendingReceiving.Count();

            }
            catch
            {
                throw new Exception("StoreServices: Unable to GetPurchaseOrderPendingAsync.");
            }
        }

        public async Task<IEnumerable<InvTrxHdr>> GetRecentTransactions(int storeId)
        {
            try {
                var today = GetCurrentDateTime().Date;

                var recentTrx = await _context.InvTrxHdrs
                    .Include(i => i.InvStore)
                    .Include(i => i.InvTrxHdrStatu)
                    .Include(i => i.InvTrxType)
                    .Include(i => i.InvTrxDtls)
                        .ThenInclude(i => i.InvItem)
                        .ThenInclude(i => i.InvUom)
                    .Where(t => t.InvStoreId == storeId && t.DtTrx.Date == today)
                    .ToListAsync();

                return recentTrx;
            }
            catch
            {
                throw new Exception("StoreServices: Unable to GetRecentTransactions.");
            }
        }

        //GET: GetCurrentDateTime()
        //PARAM: NA
        //RETURN: datetime
        //DESC: get the current datetime based on the singapore standard time
        public DateTime GetCurrentDateTime()
        {
            DateTime _localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));

            return _localTime;
        }

        public async Task<InvStore> GetStorebyIdAsync(int id)
        {
           return await _context.InvStores.FindAsync(id);
        }

        #region DBLayers

        public async Task<List<InvTrxDtl>> GetReceivedItemsAsync(int storeId)
        {
            return await _context.InvTrxDtls
               .Where(h => h.InvTrxHdr.InvTrxTypeId == TYPE_RECEIVED &&
                h.InvTrxHdr.InvStoreId == storeId)
               .Include(h => h.InvTrxHdr)
               .ToListAsync();
        }


        public async Task<List<InvTrxDtl>> GetReleasedItemsAsync(int storeId)
        {
            return await _context.InvTrxDtls
                .Where(h => h.InvTrxHdr.InvTrxTypeId == TYPE_RELEASED &&
                 h.InvTrxHdr.InvStoreId == storeId)
                .Include(h => h.InvTrxHdr)
                .ToListAsync();
        }

        public async Task<List<InvTrxDtl>> GetAdjustmentItemsAsync(int storeId)
        {
            return await _context.InvTrxDtls
                .Where(h => h.InvTrxHdr.InvTrxTypeId == TYPE_ADJUSTMENT &&
                 h.InvTrxHdr.InvStoreId == storeId)
                .Include(h => h.InvTrxHdr)
                .ToListAsync();
        }

        public async Task<List<InvTrxDtl>> GetReceivedItemsByCatAsync(int storeId, int categoryId)
        {
            return await _context.InvTrxDtls
               .Where(h => h.InvTrxHdr.InvTrxTypeId == TYPE_RECEIVED &&
                h.InvTrxHdr.InvStoreId == storeId && h.InvItem.InvCategoryId == categoryId)
               .Include(h => h.InvTrxHdr)
                .Include(h => h.InvItem)
               .ToListAsync();
        }
        public async Task<List<InvTrxDtl>> GetReleasedItemsByCatAsync(int storeId, int categoryId)
        {
            return await _context.InvTrxDtls
                .Where(h => h.InvTrxHdr.InvTrxTypeId == TYPE_RELEASED &&
                 h.InvTrxHdr.InvStoreId == storeId && h.InvItem.InvCategoryId == categoryId)
                .Include(h => h.InvTrxHdr)
                .Include(h => h.InvItem)
                .ToListAsync();
        }

        public async Task<List<InvTrxDtl>> GetAdjustmentItemsByCatAsync(int storeId, int categoryId)
        {
            return await _context.InvTrxDtls
                .Where(h => h.InvTrxHdr.InvTrxTypeId == TYPE_ADJUSTMENT &&
                 h.InvTrxHdr.InvStoreId == storeId && h.InvItem.InvCategoryId == categoryId)
                .Include(h => h.InvTrxHdr)
                .Include(h => h.InvItem)
                .ToListAsync();
        }

        public async Task<List<InvItem>> GetItemsAsync()
        {
            return await _context.InvItems
                .Include(i => i.InvWarningLevels)
                    .ThenInclude(i => i.InvWarningType)
                .Include(i => i.InvUom)
                .Include(i => i.InvItemCustomSpecs)
                    .ThenInclude(i => i.InvCustomSpec)
                .ToListAsync();
        }


        public async Task<InvCategory> GetCategoryById(int categoryId)
        {
            return await _context.InvCategories.FindAsync(categoryId);
        }

        public List<InvStore> GetStoreUsers(string user)
        {

            var storeIds = _context.InvStoreUsers.Where(s => s.InvStoreUserId == user)
                .Include(s => s.InvStore).Select(s => s.InvStoreId)
                .ToList();

            return _context.InvStores.Where(c => storeIds.Contains(c.Id)).ToList();
        }

        public string GetStoreName(int storeId)
        {
            try
            {
                var store = _context.InvStores.Find(storeId);

                return store.StoreName;
            }
            catch
            {
                return "";
            }
        }

        public decimal ConvertItemUomtoDefault(InvItem item, InvTrxDtl invTrxDtl, int itemCount)
        {
            try
            {

                //check
                if (item.InvUomId == invTrxDtl.InvUomId)
                {
                    return invTrxDtl.ItemQty;
                }

                //get convertion
                if (item.InvUomId != invTrxDtl.InvUomId)
                {
                    //check conversion table
                    var conversion = _context.InvUomConversions.Where(c => c.InvUomId_base == invTrxDtl.InvUomId
                                            && c.InvUomId_into == item.InvUomId);

                    if (conversion != null)
                    {
                        //convert 
                        var check_Conversion = conversion.First();
                        var conversion_result = check_Conversion.Factor * itemCount;
                        return conversion_result;
                    }
                }

                return -1;
            }
            catch
            {
                return -1;
            }
        }

        private decimal ConvertItemsListUoms(List<InvTrxDtl> InvTrxDtlsList)
        {
            decimal TotalConverted = 0;
            //convert to default uom and item qty
            foreach (var trx in InvTrxDtlsList)
            {

                TotalConverted += ConvertItemUomtoDefault(trx.InvItem, trx, trx.ItemQty);
            }

            return TotalConverted;
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


        public Task<List<InvCategory>> GetCategoriesList()
        {
            try
            {
                return _context.InvCategories.ToListAsync();
            }
            catch
            {
                throw new Exception("StoreServices: Unable to GetCategoriesList.");
               
            }
        }

        #endregion
    }
}
