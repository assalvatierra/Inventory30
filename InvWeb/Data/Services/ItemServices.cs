using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDBSchema.Models;
using WebDBSchema.Models.Items;
using InvWeb.Data.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace InvWeb.Data.Services
{
    public class ItemServices : IItemServices
    {
        private readonly ApplicationDbContext _context;

        private readonly int TYPE_RECEIVED = 1;
        private readonly int TYPE_RELEASED = 2;
        //private readonly int TYPE_ADJUSTMENT = 3;

        private readonly int STATUS_APPROVED = 2;
        private readonly int STATUS_CLOSED = 3;

        public ItemServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ItemLotNoSelect> GetLotNotItemList(int itemid, int storeId)
        {
            try
            {

                List<ItemLotNoSelect> lotNoSelects = new List<ItemLotNoSelect>();

                //Get Items at received with the same itemId
                var LotNoItems = _context.InvTrxDtls
                    .Include(c => c.InvItem)
                    .ThenInclude(c => c.InvUom)
                    .ThenInclude(c => c.InvWarningLevels)
                    .ThenInclude(c => c.InvWarningType)
                    .Include(c => c.InvTrxHdr)
                    .ThenInclude(c => c.InvTrxHdrStatu)
                    .Where(c => c.InvTrxHdr.InvTrxTypeId == TYPE_RECEIVED
                        && c.InvTrxHdr.InvStoreId == storeId
                        && c.InvItemId == itemid).ToList();

                if (LotNoItems == null)
                {
                    return lotNoSelects;
                }

                LotNoItems.ForEach(i => {
                    lotNoSelects.Add(new ItemLotNoSelect
                    {
                        Id     = i.Id,
                        LotNo  = i.InvTrxHdrId,
                        Description = "(" + i.InvItem.Code + ") " + i.InvItem.Description + " " + i.InvItem.Remarks,
                        Qty    = GetItemBalanceById(i.InvTrxHdrId, i.InvItemId),
                        Date   = i.InvTrxHdr.DtTrx.ToShortDateString(),
                        Status = i.InvTrxHdr.InvTrxHdrStatu.Status,
                        Uom    = i.InvUom != null ? i.InvUom.uom : "",
                        InvWarningLevels  = i.InvItem.InvWarningLevels
                    });
                });

                return lotNoSelects;
            }
            catch
            {
                throw new Exception("ItemServices: Unable to Get LotNot ItemList.");
            }
        }

        private int GetItemBalanceById(int lotNo, int itemId)
        {
            try
            {
                var trxReceived = _context.InvTrxDtls
                    .Where(i => i.InvTrxHdrId == lotNo && i.InvItemId == itemId);

                if (trxReceived != null)
                {
                    var trxLotNo = trxReceived.FirstOrDefault();
                    var trxReceiveQty = trxLotNo.ItemQty;
                    var trxReleasedQty = _context.InvTrxDtls
                        .Include(i => i.InvTrxHdr)
                        .Where(i => i.LotNo == lotNo && i.InvItemId == itemId
                                &&  i.InvTrxHdr.InvTrxTypeId == TYPE_RELEASED
                                && (i.InvTrxHdr.InvTrxHdrStatusId == STATUS_APPROVED 
                                ||  i.InvTrxHdr.InvTrxHdrStatusId == STATUS_CLOSED))
                        .Sum(c => c.ItemQty);

                    var itembalanceQty = trxReceiveQty - trxReleasedQty;

                    return itembalanceQty;
                }

                return 0;
            }
            catch
            {
                throw new Exception("ItemServices: Unable to Get Item Balance By Id");
            }
        }

        //Get Queryable Inventory Items Ordered by Category
        public IOrderedQueryable<InvItem> GetInvItemsOrderedByCategory()
        {
            return _context.InvItems.OrderBy(i => i.InvCategoryId);
        }


        //Get Select List of Inventory Items, used for Create or Edit Dropdowns List
        public SelectList GetInvItemsSelectList()
        {
            return new SelectList( _context.InvItems.OrderBy(i => i.InvCategoryId).Select(x => new
            {
                Name = String.Format("{0} - {1} {2}", x.Code, x.Description, x.Remarks),
                Value = x.Id
            }), "Value", "Name");
        }

        //Get Select List of Inventory Items, used for Create or Edit Dropdowns List with parameter selectedId
        public SelectList GetInvItemsSelectList(int selected)
        {
            return new SelectList(_context.InvItems.OrderBy(i => i.InvCategoryId).Select(x => new
            {
                Name = String.Format("{0} - {1} {2}", x.Code, x.Description, x.Remarks),
                Value = x.Id
            }), "Value", "Name", selected);
        }


        //Get Select List of Inventory Items, used for Create or Edit Dropdowns List
        public SelectList GetInStockedInvItemsSelectList(List<int> storeItems)
        {
            return new SelectList(_context.InvItems.Where(i=> storeItems.Contains(i.Id)).OrderBy(i => i.InvCategoryId).Select(x => new
            {
                Name = String.Format("{0} - {1} {2}", x.Code, x.Description, x.Remarks),
                Value = x.Id
            }), "Value", "Name");
        }


        //Get Select List of Inventory Items, used for Create or Edit Dropdowns List
        public SelectList GetInStockedInvItemsSelectList(int selected, List<int> storeItems)
        {
            return new SelectList(_context.InvItems.Where(i => storeItems.Contains(i.Id)).OrderBy(i => i.InvCategoryId).Select(x => new
            {
                Name = String.Format("{0} - {1} {2}", x.Code, x.Description, x.Remarks),
                Value = x.Id
            }), "Value", "Name", selected);
        }

        private int ItemInStockQuantity(int itemId)
        {
            try
            {

                return 0;
            }
            catch
            {
                return -1;
            }
        }

        public SelectList GetConvertableUomSelectList()
        {
            try
            {
                List<int> UomConversionList = _context.InvUomConversions.Select(c => c.InvUomId_base).ToList();

                return new SelectList(_context.InvUoms.Where(i => UomConversionList.Contains(i.Id)).Select(x => new
                {
                    Name = x.uom,
                    Value = x.Id
                }), "Value", "Name");
            }
            catch
            {
                return new SelectList(null);
            }
        }
    }
}
