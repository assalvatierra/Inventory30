﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebDBSchema.Models;
using WebDBSchema.Models.Stores;


namespace InvWeb.Data.Services
{
    public class SearchServices
    {

        private readonly ApplicationDbContext _context;


        private readonly int TYPE_RECEIVED = 1;
        private readonly int TYPE_RELEASED = 2;
        private readonly int TYPE_ADJUSTMENT = 3;

        private readonly int STATUS_REQUEST = 1;
        private readonly int STATUS_APPROVED = 2;
        //private readonly int STATUS_CANCELLED = 3;

        private readonly int OPERATOR_ADD = 1;

        public SearchServices(ApplicationDbContext context)
        {
            _context = context;
        }

        //GET : GetInvDetailsByIdAsync
        //PARAM: id (int) - 
        //RETURN: async List<InvTrxDtls> - List of Transaction Details
        //DESC: Get list of approved InvTrxDtls (Transaction Details) of specific inventory item
        public async Task<List<InvTrxDtl>> GetInvDetailsByIdAsync(int id)
        {
            return await _context.InvTrxDtls
                 .Where(c => c.InvTrxHdr.InvTrxHdrStatusId > STATUS_REQUEST && c.InvItemId == id)
                 .Include(c => c.InvItem)
                 .Include(c => c.InvTrxDtlOperator)
                 .Include(c => c.InvTrxHdr)
                    .ThenInclude(c=>c.InvStore)
                 .Include(c => c.InvTrxHdr)
                    .ThenInclude(c => c.InvTrxType)
                 .ToListAsync();
        }

        //GET : GetApprovedInvDetailsAsync
        //PARAM: NA
        //RETURN: async List<InvTrxDtls> - List of Transaction Details
        //DESC: Get list of approved InvTrxDtls (Transaction Details)
        public async Task<List<InvTrxDtl>> GetApprovedInvDetailsAsync()
        {
            return await _context.InvTrxDtls
                 .Where(i => i.InvTrxHdr.InvTrxHdrStatusId > STATUS_REQUEST)
                 .Include(i => i.InvItem)
                 .Include(i => i.InvTrxHdr)
                    .ThenInclude(i => i.InvStore)
                 .Include(i => i.InvUom)
                 .ToListAsync();
        }

        //GET: GetAvailableCountByItem/{id:int, storeId:int(optional)}
        //PARAM: id (int) - invItem Id
        //RETURN: int - total available count of the item 
        //DESC: Get the total count of the item by getting the sum of
        //      received + (-)released count + (+/-)adjustment count
        public int GetAvailableCountByItem(int id, int? storeId)
        {
            return GetReceivedCountByItem(id, storeId) + GetReleasedCountByItem(id, storeId)
                + GetAdjustedCountByItem(id, storeId);
        }

        public int GetAvailableCountByItem(int id)
        {
            return GetReceivedCountByItem(id, null) + GetReleasedCountByItem(id, null)
                + GetAdjustedCountByItem(id, null);
        }

        //GET: GetReceivedCountByItem/{id:int, storeId:int(optional)}
        //PARAM: id (int) - invItem Id, storeId (int) (optional) - store id
        //RETURN: int - total available count of the received items 
        //DESC: Get the total count of the received items
        private int GetReceivedCountByItem(int id, int? storeId)
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

        //GET: GetReleasedCountByItem/{id:int, storeId:int(optional)}
        //PARAM: id (int) - invItem Id, storeId (int) (optional) - store id
        //RETURN: int - total available count of the released items 
        //DESC: Get the total count of the received items,
        //      the total count will always result to negative count
        private int GetReleasedCountByItem(int id, int? storeId)
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


        //GET: GetAdjustedCountByItem/{id:int, storeId:int(optional)}
        //PARAM: id (int) - invItem Id, storeId (int) (optional) - store id
        //RETURN: int - total available count of the released items 
        //DESC: Get the total count of the received items,
        //      the total count will always result to negative/positve count
        //      will vary based on the operation input 
        private int GetAdjustedCountByItem(int id, int? storeId)
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

    }
}