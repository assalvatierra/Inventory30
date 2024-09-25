using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Inventory.Models.Stores;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Interfaces;
using CoreLib.DTO.InvItems;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        protected readonly int STATUS_CLOSED = 3;
        protected readonly int STATUS_CANCELLED = 4;

        protected readonly int OPERATOR_ADD = 1;
        protected readonly int OPERATOR_SUBTRACT = 2;

        public SearchServices(ApplicationDbContext context)
        {
            _context = context;
        }

        //GET : GetInvDetailsByIdAsync
        //PARAM: id (int) - 
        //RETURN: async List<InvTrxDtls> - List of Transaction Details
        //DESC: Get list of approved InvTrxDtls (Transaction Details) of specific inventory item
        public virtual async Task<IEnumerable<InvTrxDtl>> GetInvDetailsByIdAsync(int id)
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
        public virtual int GetAvailableCountByItem(int id, int? storeId)
        {
            try
            {
                //return (GetReceivedCountByItem(id, storeId) - GetReleasedCountByItem(id, storeId))
                //    + GetAdjustedCountByItem(id, storeId);

                return GetReceivedItemOnStock(id, storeId);
            }
            catch
            {
                throw new Exception("SearchServices: Unable to GetAvailableCountByItem");
            }
        }

        public virtual int GetAvailableCountByItem(int id)
        {
            try
            {
                return GetReceivedItemOnStockByItem(id);
            }
            catch
            {
                throw new Exception("SearchServices: Unable to GetAvailableCountByItem");
            }
        }

        private int GetReceivedItemOnStock(int id, int? storeId)
        {

            if(storeId == 0 || id == 0)
            {
                return 0;
            }

            var totalAvailableQty = 0;

            var itemDtls = _context.InvTrxDtls
                .Where(i =>
                       i.InvItemId == id
                    && (i.InvTrxHdr.InvTrxHdrStatusId >= STATUS_APPROVED && i.InvTrxHdr.InvTrxHdrStatusId != STATUS_CANCELLED)
                    && i.InvTrxHdr.InvStoreId == storeId)
                .ToList();

            var itemQtyAdd = itemDtls.Where(c => c.InvTrxDtlOperatorId == OPERATOR_ADD).Sum(c => c.ItemQty);
            var itemQtySub = itemDtls.Where(c=>c.InvTrxDtlOperatorId == OPERATOR_SUBTRACT).Sum(c => c.ItemQty);

            totalAvailableQty = itemQtyAdd - itemQtySub;

            return totalAvailableQty;
        }


        private int GetReceivedItemOnStockByItem(int id)
        {

            if (id == 0)
            {
                return 0;
            }

            var totalAvailableQty = 0;

            var itemDtls = _context.InvTrxDtls
                .Where(i =>
                       i.InvItemId == id &&
                      (i.InvTrxHdr.InvTrxHdrStatusId >= STATUS_APPROVED && i.InvTrxHdr.InvTrxHdrStatusId != STATUS_CANCELLED))
                .ToList();

            var itemQtyAdd = itemDtls.Where(c => c.InvTrxDtlOperatorId == OPERATOR_ADD).Sum(c => c.ItemQty);
            var itemQtySub = itemDtls.Where(c => c.InvTrxDtlOperatorId == OPERATOR_SUBTRACT).Sum(c => c.ItemQty);

            totalAvailableQty = itemQtyAdd - itemQtySub;

            return totalAvailableQty;
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



        public async Task<List<InvItemSearch>> GetItemsOnStock()
        {

           

            const string Sqldata = "\r\nSELECT * \r\nFROM\r\n(\r\n\tselect item.Id, \r\n\titem.Id as 'MasterId'\r\n\t,inv.Id as 'ItemId'\r\n\t,item.ItemQty\r\n\t,dataRel.Qty as 'ReleasedQty'\r\n\t,dataHold.Qty as 'ItemOnHoldQty'\r\n\t,(item.ItemQty - coalesce(dataRel.Qty,0) )as 'StockOnHand' \r\n\t,(item.ItemQty - coalesce(dataRel.Qty,0) - coalesce(dataHold.Qty,0) ) as 'AvailableQty' \r\n\t,itemcat.Description as 'Category'\r\n\t,inv.Code\r\n\t,inv.Description\r\n\t,item.BatchNo\r\n\t,item.LotNo\r\n\t,brand.Name as 'Brand'\r\n\t,origin.Name as 'Origin'\r\n\t,store.StoreName\r\n\t,area.Name as 'Location'\r\n\r\n\tfrom dbo.InvItemMasters item\r\n\tinner join dbo.InvItems inv on inv.Id = item.InvItemId\r\n\tleft join dbo.InvCategories itemcat on itemcat.Id = inv.InvCategoryId\r\n\tleft join dbo.InvItemBrands brand on brand.Id = item.InvItemBrandId\r\n\tleft join dbo.InvItemOrigins origin on origin.Id = item.InvItemOriginId\r\n\tleft join dbo.InvStoreAreas area on area.Id = item.InvStoreAreaId\r\n\tleft join dbo.InvStores store on store.Id = area.InvStoreId\r\n\tleft join (\r\n\t\tselect dtlxitem.InvItemMasterId as 'MasterItemId', SUM(trxdtls.ItemQty) as 'Qty'\r\n\t\tfrom dbo.InvTrxDtls trxdtls \r\n\t\tinner join dbo.InvTrxDtlxItemMasters dtlxitem on dtlxitem.InvTrxDtlId = trxdtls.Id\r\n\t\tinner join dbo.InvTrxHdrs trxHdr on trxHdr.Id = trxdtls.InvTrxHdrId\r\n\t\tinner join dbo.InvTrxHdrStatus trxStatus on trxStatus.Id = trxHdr.InvTrxHdrStatusId\r\n\t\tinner join dbo.InvTrxDtlOperators trxdtlop on trxdtlop.Id = trxdtls.InvTrxDtlOperatorId\r\n\t\tinner join dbo.InvTrxTypes trxtype on trxtype.Id = trxHdr.InvTrxTypeId\r\n\t\twhere trxtype.Type = 'Release' \r\n\t\tand trxStatus.Status = 'Closed'\r\n\t\tgroup by dtlxitem.InvItemMasterId\r\n\t\t) dataRel on dataRel.MasterItemId = item.Id\r\n\tleft join (\r\n\t\tselect dtlxitem.InvItemMasterId as 'MasterItemId', SUM(trxdtls.ItemQty) as 'Qty'\r\n\t\tfrom dbo.InvTrxDtls trxdtls \r\n\t\tinner join dbo.InvTrxDtlxItemMasters dtlxitem on dtlxitem.InvTrxDtlId = trxdtls.Id\r\n\t\tinner join dbo.InvTrxHdrs trxHdr on trxHdr.Id = trxdtls.InvTrxHdrId\r\n\t\tinner join dbo.InvTrxHdrStatus trxStatus on trxStatus.Id = trxHdr.InvTrxHdrStatusId\r\n\t\tinner join dbo.InvTrxDtlOperators trxdtlop on trxdtlop.Id = trxdtls.InvTrxDtlOperatorId\r\n\t\tinner join dbo.InvTrxTypes trxtype on trxtype.Id = trxHdr.InvTrxTypeId\r\n\t\twhere trxtype.Type = 'Release' \r\n\t\tand trxStatus.Status = 'Approve'\r\n\t\tgroup by dtlxitem.InvItemMasterId\r\n\t\t) dataHold on dataRel.MasterItemId = item.Id\r\n\r\n) Item \r\nwhere item.AvailableQty > 0\r\n--AND ( @itemdesc is null or RTRIM(@itemdesc)='' or charindex(@itemdesc, item.Description)>0 )\r\n--AND ( @origin is null or RTRIM(@origin)='' or charindex(@origin, item.Origin)>0 )\r\n--AND ( @brand is null or RTRIM(@brand)='' or charindex(@brand, item.Brand)>0 )\r\n--AND ( @store is null or RTRIM(@store)='' or charindex(@store, item.StoreName)>0 )\r\n\r\norder by Item.ItemId\r\n\r\n;\r\n\r\n\r\n\r\n\r\n\r\n";
            var items = await _context.InvItemSearchs
                .FromSqlRaw(Sqldata).ToListAsync();
//"SELECT * FROM " +
//"( "+
//"	select"+
//"	item.Id as 'MasterId'"+
//"	,inv.Id as 'ItemId'"+
//"	,item.ItemQty"+
//"	,dataRel.Qty as 'ReleasedQty'"+
//"	,dataHold.Qty as 'ItemOnHoldQty'"+
//"	,(item.ItemQty - coalesce(dataRel.Qty,0) )as 'StockOnHand'"+
//"	,(item.ItemQty - coalesce(dataRel.Qty,0) - coalesce(dataHold.Qty,0) ) as 'AvailableQty'"+
//"	,itemcat.Description as 'Category'"+
//"	,inv.Code"+
//"	,inv.Description"+
//"	,item.BatchNo"+
//"	,item.LotNo"+
//"   , brand.Name as 'Brand'"+
//"	,origin.Name as 'Origin'"+
//"	,store.StoreName"+
//"	,area.Name as 'Location'"+
//""+
//"	from dbo.InvItemMasters item"+
//"	inner join dbo.InvItems inv on inv.Id = item.InvItemId"+
//"	left join dbo.InvCategories itemcat on itemcat.Id = inv.InvCategoryId"+
//"	left join dbo.InvItemBrands brand on brand.Id = item.InvItemBrandId"+
//"	left join dbo.InvItemOrigins origin on origin.Id = item.InvItemOriginId"+
//"	left join dbo.InvStoreAreas area on area.Id = item.InvStoreAreaId"+
//"	left join dbo.InvStores store on store.Id = area.InvStoreId"+
//"	left join ("+
//"		select dtlxitem.InvItemMasterId as 'MasterItemId', SUM(trxdtls.ItemQty) as 'Qty'"+
//"		from dbo.InvTrxDtls trxdtls"+
//"		inner join dbo.InvTrxDtlxItemMasters dtlxitem on dtlxitem.InvTrxDtlId = trxdtls.Id"+
//"		inner join dbo.InvTrxHdrs trxHdr on trxHdr.Id = trxdtls.InvTrxHdrId"+
//"		inner join dbo.InvTrxHdrStatus trxStatus on trxStatus.Id = trxHdr.InvTrxHdrStatusId"+
//"		inner join dbo.InvTrxDtlOperators trxdtlop on trxdtlop.Id = trxdtls.InvTrxDtlOperatorId"+
//"		inner join dbo.InvTrxTypes trxtype on trxtype.Id = trxHdr.InvTrxTypeId"+
//"		where trxtype.Type = 'Release'"+
//"		and trxStatus.Status = 'Closed'"+
//"		group by dtlxitem.InvItemMasterId"+
//"		) dataRel on dataRel.MasterItemId = item.Id"+
//"	left join ("+
//"		select dtlxitem.InvItemMasterId as 'MasterItemId', SUM(trxdtls.ItemQty) as 'Qty'"+
//"		from dbo.InvTrxDtls trxdtls"+
//"		inner join dbo.InvTrxDtlxItemMasters dtlxitem on dtlxitem.InvTrxDtlId = trxdtls.Id"+
//"		inner join dbo.InvTrxHdrs trxHdr on trxHdr.Id = trxdtls.InvTrxHdrId"+
//"		inner join dbo.InvTrxHdrStatus trxStatus on trxStatus.Id = trxHdr.InvTrxHdrStatusId"+
//"		inner join dbo.InvTrxDtlOperators trxdtlop on trxdtlop.Id = trxdtls.InvTrxDtlOperatorId"+
//"		inner join dbo.InvTrxTypes trxtype on trxtype.Id = trxHdr.InvTrxTypeId"+
//"		where trxtype.Type = 'Release'"+
//"		and trxStatus.Status = 'Approve'"+
//"		group by dtlxitem.InvItemMasterId"+
//"        ) dataHold on dataRel.MasterItemId = item.Id"+
//""+
//") Item"+
//"where item.AvailableQty > 0"+
//            "order by Item.ItemId" +
//            ";").ToListAsync();


            //return _context. <InvItemSearch>(query).tolist();

            return items;

        }



    }


}
