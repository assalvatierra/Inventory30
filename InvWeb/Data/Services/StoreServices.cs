using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvWeb.Data.Interfaces;
using InvWeb.Data;
using Microsoft.EntityFrameworkCore;
using WebDBSchema.Models;
using WebDBSchema.Models.Stores;
using Microsoft.Extensions.Logging;

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

        public async Task<IEnumerable<StoreInvCount>> GetStoreItemsSummary(int storeId, string orderBy)
        {
            try
            {

            int accepted  = 0;
            int pending   = 0;
            int released  = 0;
            int requested = 0;

            var invItems = await GetItemsAsync();

            //Todo: add filter to add only trx with approved status (statusId = 1) 
            var Received = await this.GetReceivedItemsAsync(storeId);
            var Released = await this.GetReleasedItemsAsync(storeId);
            var Adjustment = await this.GetAdjustmentItemsAsync(storeId);

            List<StoreInvCount> storeInvItems = new();

            foreach (var item in invItems)
            {
                int itemReceived = Received.Where(h => h.InvItemId == item.Id).Sum(i => i.ItemQty);
                int itemReleased = Released.Where(h => h.InvItemId == item.Id).Sum(i => i.ItemQty);

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
                    var itemDetails = invItems.Where(i => i.Id == item.Id).FirstOrDefault();
                    storeInvItems.Add(new StoreInvCount
                    {
                        Id = item.Id,
                        Description = "(" + itemDetails.Code + ") " + itemDetails.Description + " " + itemDetails.Remarks,
                        Available = (itemReceived - itemReleased) + (itemAdjustment),
                        OnHand = (accepted - released) + (itemAdjustment),
                        ReceivePending = pending,
                        ReceiveAccepted = accepted,
                        ReleaseRequest = requested,
                        ReleaseReleased = released,
                        Adjustments = itemAdjustment,
                        InvWarningLevels = itemDetails.InvWarningLevels,
                        Category = item.InvCategory != null ? item.InvCategory.Description : null,
                        CategoryId = item.InvCategory != null ? item.InvCategoryId : 0
                    });
                }

                if (Released.Where(h => h.InvItemId == item.Id).Any() && !Received.Where(h => h.InvItemId == item.Id).Any())
                {
                    var itemDetails = invItems.Where(i => i.Id == item.Id).FirstOrDefault();
                    storeInvItems.Add(new StoreInvCount
                    {
                        Id = item.Id,
                        Description = "(" + itemDetails.Code + ") " + itemDetails.Description + " " + itemDetails.Remarks,
                        Available = (itemReceived - itemReleased) + (itemAdjustment),
                        OnHand = (accepted - released) + (itemAdjustment),
                        ReceivePending = pending,
                        ReceiveAccepted = accepted,
                        ReleaseRequest = requested,
                        ReleaseReleased = released,
                        Adjustments = itemAdjustment,
                        InvWarningLevels = itemDetails.InvWarningLevels,
                        Category = item.InvCategory != null ? item.InvCategory.Description : null,
                        CategoryId = item.InvCategory != null ? item.InvCategoryId : 0
                    });
                }

            }

                _logger.LogInformation("StoreServices : GetStoreItemsSummary ");

                _logger.LogInformation("GetInvCountOrderBy : " + orderBy);
                return GetInvCountOrderBy(storeInvItems, orderBy);


            }
            catch
            {
                _logger.LogError("StoreServices: Unable to GetStoreItemsSummary");
                throw new Exception("StoreServices: Unable to GetStoreItemsSummary.");
            }
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

        public int GetPendingItemsCount(List<InvTrxDtl> receivedItems, int itemId)
        {
            return receivedItems.Where(h => h.InvTrxHdr.InvTrxHdrStatusId == 1 &&
                                            h.InvItemId == itemId).ToList()
                                            .Sum(c => c.ItemQty);
        }

        public int GetAcceptedItemsCount(List<InvTrxDtl> receivedItems, int itemId)
        {
            return receivedItems.Where(h => h.InvTrxHdr.InvTrxHdrStatusId > 1 &&
                                            h.InvItemId == itemId).ToList()
                                            .Sum(c => c.ItemQty);
        }


        public int GetRequestedItemsCount(List<InvTrxDtl> releasedItems, int itemId)
        {
            return releasedItems.Where(h => h.InvTrxHdr.InvTrxHdrStatusId == 1 &&
                                            h.InvItemId == itemId).ToList()
                                            .Sum(c => c.ItemQty);
        }

        public int GetReleasedItemsCount(List<InvTrxDtl> releasedItems, int itemId)
        {
            return releasedItems.Where(h => h.InvTrxHdr.InvTrxHdrStatusId > 1 &&
                                            h.InvItemId == itemId).ToList()
                                            .Sum(c => c.ItemQty);
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
                    .Where(t => t.InvStoreId == storeId &&  t.InvPoHdrStatusId == 1)
                    .ToListAsync();
                return storePendingReceiving.Count();

            }
            catch
            {
                throw new Exception("StoreServices: Unable to GetPurchaseOrderPendingAsync.");
            }
        }

        public async Task<List<InvTrxHdr>> GetRecentTransactions(int storeId)
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
                    .Where(t => t.InvStoreId == storeId && t.DtTrx.Date == today).ToListAsync();

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


        public async Task<List<InvItem>> GetItemsAsync()
        {
            return await _context.InvItems
                .Include(c=>c.InvWarningLevels)
                .ThenInclude(c=>c.InvWarningType)
                .ToListAsync();
        }

        public List<InvStore> GetStoreUsers(string user)
        {

            var storeIds = _context.InvStoreUsers.Where(s => s.InvStoreUserId == user)
                .Include(s=>s.InvStore).Select(s=>s.InvStoreId)
                .ToList();

            return _context.InvStores.Where(c => storeIds.Contains(c.Id)).ToList();
        }

        #endregion
    }
}
