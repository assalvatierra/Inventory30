using CoreLib.DTO.InvItems;
using CoreLib.Inventory.Interfaces;
using CoreLib.Inventory.Models.Items;
using CoreLib.Models.Inventory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvWeb.Pages.xTestPages.Search
{
    public class IndexModel : PageModel
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

        public IndexModel(ApplicationDbContext context, ISearchServices iservices)
        {
            _context = context;
            services = iservices;
        }

        public List<InvItemSearch> ItemSearchResults { get; set; }

        public void OnGet()
        {
            ItemSearchResults = new List<InvItemSearch>();

            ItemSearchResults =  GetItemsOnStock();
        }



        public List<InvItemSearch> GetItemsOnStock()
        {
            const string Sqldata = "\r\nSELECT * \r\nFROM\r\n(\r\n\tselect item.Id, \r\n\titem.Id as 'MasterId'\r\n\t,inv.Id as 'ItemId'\r\n\t,item.ItemQty\r\n\t,dataRel.Qty as 'ReleasedQty'\r\n\t,dataHold.Qty as 'ItemOnHoldQty'\r\n\t,(item.ItemQty - coalesce(dataRel.Qty,0) )as 'StockOnHand' \r\n\t,(item.ItemQty - coalesce(dataRel.Qty,0) - coalesce(dataHold.Qty,0) ) as 'AvailableQty' \r\n\t,itemcat.Description as 'Category'\r\n\t,inv.Code\r\n\t,inv.Description\r\n\t,item.BatchNo\r\n\t,item.LotNo\r\n\t,brand.Name as 'Brand'\r\n\t,origin.Name as 'Origin'\r\n\t,store.StoreName\r\n\t,area.Name as 'Location'\r\n\r\n\tfrom dbo.InvItemMasters item\r\n\tinner join dbo.InvItems inv on inv.Id = item.InvItemId\r\n\tleft join dbo.InvCategories itemcat on itemcat.Id = inv.InvCategoryId\r\n\tleft join dbo.InvItemBrands brand on brand.Id = item.InvItemBrandId\r\n\tleft join dbo.InvItemOrigins origin on origin.Id = item.InvItemOriginId\r\n\tleft join dbo.InvStoreAreas area on area.Id = item.InvStoreAreaId\r\n\tleft join dbo.InvStores store on store.Id = area.InvStoreId\r\n\tleft join (\r\n\t\tselect dtlxitem.InvItemMasterId as 'MasterItemId', SUM(trxdtls.ItemQty) as 'Qty'\r\n\t\tfrom dbo.InvTrxDtls trxdtls \r\n\t\tinner join dbo.InvTrxDtlxItemMasters dtlxitem on dtlxitem.InvTrxDtlId = trxdtls.Id\r\n\t\tinner join dbo.InvTrxHdrs trxHdr on trxHdr.Id = trxdtls.InvTrxHdrId\r\n\t\tinner join dbo.InvTrxHdrStatus trxStatus on trxStatus.Id = trxHdr.InvTrxHdrStatusId\r\n\t\tinner join dbo.InvTrxDtlOperators trxdtlop on trxdtlop.Id = trxdtls.InvTrxDtlOperatorId\r\n\t\tinner join dbo.InvTrxTypes trxtype on trxtype.Id = trxHdr.InvTrxTypeId\r\n\t\twhere trxtype.Type = 'Release' \r\n\t\tand trxStatus.Status = 'Closed'\r\n\t\tgroup by dtlxitem.InvItemMasterId\r\n\t\t) dataRel on dataRel.MasterItemId = item.Id\r\n\tleft join (\r\n\t\tselect dtlxitem.InvItemMasterId as 'MasterItemId', SUM(trxdtls.ItemQty) as 'Qty'\r\n\t\tfrom dbo.InvTrxDtls trxdtls \r\n\t\tinner join dbo.InvTrxDtlxItemMasters dtlxitem on dtlxitem.InvTrxDtlId = trxdtls.Id\r\n\t\tinner join dbo.InvTrxHdrs trxHdr on trxHdr.Id = trxdtls.InvTrxHdrId\r\n\t\tinner join dbo.InvTrxHdrStatus trxStatus on trxStatus.Id = trxHdr.InvTrxHdrStatusId\r\n\t\tinner join dbo.InvTrxDtlOperators trxdtlop on trxdtlop.Id = trxdtls.InvTrxDtlOperatorId\r\n\t\tinner join dbo.InvTrxTypes trxtype on trxtype.Id = trxHdr.InvTrxTypeId\r\n\t\twhere trxtype.Type = 'Release' \r\n\t\tand trxStatus.Status = 'Approve'\r\n\t\tgroup by dtlxitem.InvItemMasterId\r\n\t\t) dataHold on dataRel.MasterItemId = item.Id\r\n\r\n) Item \r\nwhere item.AvailableQty > 0\r\n--AND ( @itemdesc is null or RTRIM(@itemdesc)='' or charindex(@itemdesc, item.Description)>0 )\r\n--AND ( @origin is null or RTRIM(@origin)='' or charindex(@origin, item.Origin)>0 )\r\n--AND ( @brand is null or RTRIM(@brand)='' or charindex(@brand, item.Brand)>0 )\r\n--AND ( @store is null or RTRIM(@store)='' or charindex(@store, item.StoreName)>0 )\r\n\r\norder by Item.ItemId\r\n;\r\n";
            
            var items = _context.InvItemSearchs.FromSqlRaw(Sqldata).ToList();

            return items;


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


        }

    }
}
