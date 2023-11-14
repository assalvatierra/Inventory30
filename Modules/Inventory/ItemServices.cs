using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLib.Inventory.Models;
using CoreLib.Inventory.Models.Items;
using CoreLib.Inventory.Interfaces;
using Microsoft.Extensions.Logging;
using CoreLib.Models.Inventory;
using System.Collections;

namespace Modules.Inventory
{
    public class ItemServices : IItemServices
    {
        private readonly ApplicationDbContext _context;

        private readonly int TYPE_RECEIVED = 1;
        private readonly int TYPE_RELEASED = 2;

        private readonly int STATUS_APPROVED = 2;
        private readonly int STATUS_CLOSED = 3;

        public ItemServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual IEnumerable<ItemLotNoSelect> GetLotNotItemList(int itemid, int storeId)
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
                        InvWarningLevels  = i.InvItem.InvWarningLevels,
                        BatchNo = i.BatchNo
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
        public virtual IOrderedQueryable<InvItem> GetInvItemsOrderedByCategory()
        {
            return _context.InvItems.OrderBy(i => i.InvCategoryId);
        }


        //Get Select List of Inventory Items, used for Create or Edit Dropdowns List
        public virtual IOrderedQueryable<InvItem> GetInvItemsSelectList()
        {
            return _context.InvItems.OrderBy(i => i.InvCategoryId);

        }

        //Get Select List of Inventory Items, used for Create or Edit Dropdowns List with parameter selectedId
        public virtual IOrderedQueryable<InvItem> GetInvItemsSelectList(int selected)
        {
            return _context.InvItems.OrderBy(i => i.InvCategoryId) ;
        }


        //Get Select List of Inventory Items, used for Create or Edit Dropdowns List
        public virtual IOrderedQueryable<InvItem> GetInStockedInvItemsSelectList(List<int> storeItems)
        {
            return _context.InvItems.Where(i=> storeItems.Contains(i.Id))
                .OrderBy(i => i.InvCategoryId);
        }


        //Get Select List of Inventory Items, used for Create or Edit Dropdowns List
        public virtual IOrderedQueryable<InvItem> GetInStockedInvItemsSelectList(int selected, List<int> storeItems)
        {
            return _context.InvItems.Where(i => storeItems.Contains(i.Id))
                .OrderBy(i => i.InvCategoryId);
           
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

        public virtual IQueryable<InvUom> GetConvertableUomSelectList()
        {
            try
            {
                List<int> UomConversionList = _context.InvUomConversions.Select(c => c.InvUomId_base).ToList();

                return _context.InvUoms.Where(i => UomConversionList.Contains(i.Id));

            }
            catch
            {
                throw new Exception("ItemServices: Unable to Get Uom list");
            }
        }

        public virtual IList<InvItem> GetInvItemsWithSteelSpecs()
        {
            return _context.InvItems.OrderBy(i => i.InvCategoryId)
                 .Include(i => i.InvCategory)
                 .Include(i => i.InvItemSpec_Steel)
                    .ThenInclude(i=>i.SteelMainCat)
                 .Include(i => i.InvItemSpec_Steel)
                    .ThenInclude(i => i.SteelBrand)
                 .Include(i => i.InvItemSpec_Steel)
                    .ThenInclude(i => i.SteelOrigin)
                 .Include(i => i.InvItemSpec_Steel)
                    .ThenInclude(i => i.SteelSubCat)
                 .Include(i => i.InvItemSpec_Steel)
                    .ThenInclude(i => i.SteelMaterialGrade)
                 .Include(i => i.InvItemSpec_Steel)
                    .ThenInclude(i => i.SteelMaterial)
                 .Include(i => i.InvItemSpec_Steel)
                    .ThenInclude(i => i.SteelSize)
                 .ToList();
        }
    }
}
