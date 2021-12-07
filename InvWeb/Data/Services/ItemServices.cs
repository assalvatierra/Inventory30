using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDBSchema.Models;
using WebDBSchema.Models.Items;

namespace InvWeb.Data.Services
{
    public class ItemServices
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
                        Uom    = i.InvUom.uom ,
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
    }
}
