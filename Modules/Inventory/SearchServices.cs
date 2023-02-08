using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Inventory.Models.Stores;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Interfaces;

namespace Modules.Inventory
{
    public class SearchServices: ISearchServices
    {

        protected readonly ApplicationDbContext _context;


        protected readonly int TYPE_RECEIVED = 1;
        protected readonly int TYPE_RELEASED = 2;
        protected readonly int TYPE_ADJUSTMENT = 3;

        protected readonly int STATUS_REQUEST = 1;
        protected readonly int STATUS_APPROVED = 2;
        //protected readonly int STATUS_CANCELLED = 3;

        protected readonly int OPERATOR_ADD = 1;

        public SearchServices(ApplicationDbContext context)
        {
            _context = context;
        }

        //GET : GetInvDetailsByIdAsync
        //PARAM: id (int) - 
        //RETURN: async List<InvTrxDtls> - List of Transaction Details
        //DESC: Get list of approved InvTrxDtls (Transaction Details) of specific inventory item
        public async Task<IEnumerable<InvTrxDtl>> GetInvDetailsByIdAsync(int id)
        {
            try
            {
                return await _context.InvTrxDtls
                    .Where(c => c.InvTrxHdr.InvTrxHdrStatusId > STATUS_REQUEST && c.InvItemId == id)
                    .Include(c => c.InvItem)
                        .ThenInclude(c => c.InvUom)
                        .ThenInclude(c => c.InvWarningLevels)
                        .ThenInclude(c => c.InvWarningType)
                    .Include(c => c.InvTrxDtlOperator)
                    .Include(c => c.InvTrxHdr)
                       .ThenInclude(c => c.InvStore)
                    .Include(c => c.InvTrxHdr)
                       .ThenInclude(c => c.InvTrxType)
                    .ToListAsync();
            }
            catch
            {
                throw new Exception("SearchServices: Unable to GetInvDetailsByIdAsync");
            }

        }

        //GET : GetApprovedInvDetailsAsync
        //PARAM: NA
        //RETURN: async List<InvTrxDtls> - List of Transaction Details
        //DESC: Get list of approved InvTrxDtls (Transaction Details)
        public virtual async Task<IEnumerable<InvTrxDtl>> GetApprovedInvDetailsAsync()
        {
            try
            {

                return await _context.InvTrxDtls
                     .Where(i => i.InvTrxHdr.InvTrxHdrStatusId > STATUS_REQUEST)
                     .Include(i => i.InvItem)
                     .Include(i => i.InvTrxHdr)
                        .ThenInclude(i => i.InvStore)
                     .Include(i => i.InvUom)
                     .ToListAsync();
            }
            catch
            {
                throw new Exception("SearchServices: Unable to GetApprovedInvDetailsAsync");
            }
        }

        //GET: GetAvailableCountByItem/{id:int, storeId:int(optional)}
        //PARAM: id (int) - invItem Id
        //RETURN: int - total available count of the item 
        //DESC: Get the total count of the item by getting the sum of
        //      received + (-)released count + (+/-)adjustment count
        public int GetAvailableCountByItem(int id, int? storeId)
        {
            try
            {
                return GetReceivedCountByItem(id, storeId) + GetReleasedCountByItem(id, storeId)
                    + GetAdjustedCountByItem(id, storeId);
            }
            catch
            {
                throw new Exception("SearchServices: Unable to GetAvailableCountByItem");
            }
        }

        public int GetAvailableCountByItem(int id)
        {
            try
            {
                return GetReceivedCountByItem(id, null) + GetReleasedCountByItem(id, null)
                    + GetAdjustedCountByItem(id, null);
            }
            catch
            {
                throw new Exception("SearchServices: Unable to GetAvailableCountByItem");
            }
        }

        //GET: GetReceivedCountByItem/{id:int, storeId:int(optional)}
        //PARAM: id (int) - invItem Id, storeId (int) (optional) - store id
        //RETURN: int - total available count of the received items 
        //DESC: Get the total count of the received items
        private int GetReceivedCountByItem(int id, int? storeId)
        {
            try
            {
                var totalQty = 0;

                var itemDtls = _context.InvTrxDtls
                    .Where(i =>
                           i.InvItemId == id
                        && i.InvTrxHdr.InvTrxHdrStatusId >= STATUS_APPROVED
                        && i.InvTrxHdr.InvTrxTypeId == TYPE_RECEIVED)
                    .ToList();

                //if get items details by storeId
                if (storeId != null)
                {
                    itemDtls = itemDtls
                    .Where(i => i.InvTrxHdr.InvStoreId == storeId)
                    .ToList();
                }

                //acquire the total count
                foreach (var item in itemDtls)
                {
                    totalQty += item.ItemQty;
                };

                return totalQty;
            }
            catch
            {

                throw new Exception("SearchServices: Unable to GetReceivedCountByItem");
            }


        }

        //GET: GetReleasedCountByItem/{id:int, storeId:int(optional)}
        //PARAM: id (int) - invItem Id, storeId (int) (optional) - store id
        //RETURN: int - total available count of the released items 
        //DESC: Get the total count of the received items,
        //      the total count will always result to negative count
        private int GetReleasedCountByItem(int id, int? storeId)
        {
            try
            {
                var totalQty = 0;

                var itemDtls = _context.InvTrxDtls
                    .Where(i => i.InvItemId == id
                        && i.InvTrxHdr.InvTrxHdrStatusId >= STATUS_APPROVED
                        && i.InvTrxHdr.InvTrxTypeId == TYPE_RELEASED)
                    .ToList();

                if (storeId != null)
                {
                    itemDtls = itemDtls
                    .Where(i => i.InvTrxHdr.InvStoreId == storeId)
                    .ToList();
                }

                foreach (var item in itemDtls)
                {
                    totalQty -= item.ItemQty;
                };

                return totalQty;
            }
            catch
            {

                throw new Exception("SearchServices: Unable to GetReleasedCountByItem");
            }
        }


        //GET: GetAdjustedCountByItem/{id:int, storeId:int(optional)}
        //PARAM: id (int) - invItem Id, storeId (int) (optional) - store id
        //RETURN: int - total available count of the released items 
        //DESC: Get the total count of the received items,
        //      the total count will always result to negative/positve count
        //      will vary based on the operation input 
        private int GetAdjustedCountByItem(int id, int? storeId)
        {
            try
            {
                var totalQty = 0;

                //get list of transactions with approved and adjustment 
                var itemDtls = _context.InvTrxDtls
                    .Where(i => i.InvItemId == id
                        && i.InvTrxHdr.InvTrxHdrStatusId >= STATUS_APPROVED
                        && i.InvTrxHdr.InvTrxTypeId == TYPE_ADJUSTMENT)
                    .ToList();

                //if store is given
                if (storeId != null)
                {
                    itemDtls = itemDtls
                    .Where(i => i.InvTrxHdr.InvStoreId == storeId)
                    .ToList();
                }

                foreach (var item in itemDtls)
                {
                    //check transaction operation
                    if (item.InvTrxDtlOperatorId == OPERATOR_ADD)
                    {
                        totalQty += item.ItemQty;
                    }
                    else
                    {
                        totalQty -= item.ItemQty;
                    }
                };

                return totalQty;
            }
            catch
            {
                throw new Exception("SearchServices: Unable to GetAdjustedCountByItem");
            }
        }

    }
}
