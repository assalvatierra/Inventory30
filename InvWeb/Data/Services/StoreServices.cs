using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvWeb.Data.Interfaces;
using InvWeb.Data;
using Microsoft.EntityFrameworkCore;
using WebDBSchema.Models;
using WebDBSchema.Models.Stores;

namespace InvWeb.Data.Services
{
    public class StoreServices : IStoreServices
    {
        private readonly ApplicationDbContext _context;


        private readonly int TYPE_RECEIVED = 1;
        private readonly int TYPE_RELEASED = 2;
        private readonly int TYPE_ADJUSTMENT = 3;

        public StoreServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StoreInvCount>> GetStoreItemsSummary(int storeId)
        {
            try
            {

            int accepted = 0;
            int pending = 0;
            int released = 0;
            int requested = 0;

            var invItems = await GetItemsAsync();

            //Todo: add filter to add only trx with approved status (statusId = 1) 
            var Received = await this.GetReceivedItemsAsync(storeId);
            var Released = await this.GetReleasedItemsAsync(storeId);
            var Adjustment = await this.GetAdjustmentItemsAsync(storeId);

            List<StoreInvCount> storeInvItems = new();

            foreach (var item in invItems.Select(i => i.Id))
            {
                int itemReceived = Received.Where(h => h.InvItemId == item).Sum(i => i.ItemQty);
                int itemReleased = Released.Where(h => h.InvItemId == item).Sum(i => i.ItemQty);

                if (Received != null)
                {
                    accepted = GetAcceptedItemsCount(Received, item);
                    pending = GetPendingItemsCount(Received, item);
                }

                if (Released != null)
                {
                    released = GetReleasedItemsCount(Released, item);
                    requested = GetRequestedItemsCount(Released, item);
                }

                int itemAdjustment = GetAdjustmentItemsCount(Adjustment, item);

                if (Received.Where(h => h.InvItemId == item).Any())
                {
                    var itemDetails = invItems.Where(i => i.Id == item).FirstOrDefault();
                    storeInvItems.Add(new StoreInvCount
                    {
                        Id = item,
                        Description = "(" + itemDetails.Code + ") " + itemDetails.Description + " " + itemDetails.Remarks,
                        Available = (itemReceived - itemReleased) + (itemAdjustment),
                        OnHand = (accepted - released) + (itemAdjustment),
                        ReceivePending = pending,
                        ReceiveAccepted = accepted,
                        ReleaseRequest = requested,
                        ReleaseReleased = released,
                        Adjustments = itemAdjustment
                    });
                }

                if (Released.Where(h => h.InvItemId == item).Any() && !Received.Where(h => h.InvItemId == item).Any())
                {
                    var itemDetails = invItems.Where(i => i.Id == item).FirstOrDefault();
                    storeInvItems.Add(new StoreInvCount
                    {
                        Id = item,
                        Description = "(" + itemDetails.Code + ") " + itemDetails.Description + " " + itemDetails.Remarks,
                        Available = (itemReceived - itemReleased) + (itemAdjustment),
                        OnHand = (accepted - released) + (itemAdjustment),
                        ReceivePending = pending,
                        ReceiveAccepted = accepted,
                        ReleaseRequest = requested,
                        ReleaseReleased = released,
                        Adjustments = itemAdjustment
                    });
                }

            }

            return storeInvItems;
            }
            catch
            {
                throw new Exception("StoreServices: Unable to GetStoreItemsSummary.");
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
            return await _context.InvItems.ToListAsync();
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
