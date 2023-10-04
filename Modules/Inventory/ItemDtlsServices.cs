using CoreLib.DTO.Common.TrxDetails;
using CoreLib.DTO.Receiving;
using CoreLib.DTO.Releasing;
using CoreLib.Inventory.Interfaces;
using CoreLib.Inventory.Models;
using CoreLib.Inventory.Models.Items;
using CoreLib.Models.Inventory;
using Inventory.DBAccess;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Inventory
{
    public class ItemDtlsServices : IItemDtlsServices
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly DBMasterService dbMaster;
        private readonly IUomServices uomServices;
        private readonly IItemServices itemServices;
        private readonly IStoreServices storeServices;
        private readonly IItemTrxServices itemTrxServices;

        private readonly int TYPE_RELEASING = 2;
        private readonly int OPERATION_ADD = 1;
        private readonly int OPERATION_SUBTRACT = 2;

        public ItemDtlsServices(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;

            dbMaster = new DBMasterService(_context, _logger);
            uomServices = new UomServices(_context);
            itemServices = new ItemServices(_context);
            storeServices = new StoreServices(_context, _logger);
            itemTrxServices = new ItemTrxServices(_context, _logger);
        }

        public virtual void CreateInvDtls(InvTrxDtl invTrxDtl)
        {
            try
            {
                _context.InvTrxDtls.Add(invTrxDtl);
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemDtlsServices: Unable to CreateInvDtls :" + ex.Message);
                throw new Exception("ItemDtlsServices: Unable to CreateInvDtls :" + ex.Message);
            }
        }

        public virtual void DeleteInvDtls(InvTrxDtl invTrxDtl)
        {
            try
            {
                _context.InvTrxDtls.Remove(invTrxDtl);
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemDtlsServices: Unable to DeleteInvDtls :" + ex.Message);
                throw new Exception("ItemDtlsServices: Unable to DeleteInvDtls :" + ex.Message);
            }
        }

        public virtual void EditInvDtls(InvTrxDtl invTrxDtl)
        {
            try
            {

                _context.Attach(invTrxDtl).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemDtlsServices: Unable to EditInvDtls :" + ex.Message);
                throw new Exception("ItemDtlsServices: Unable to EditInvDtls :" + ex.Message);
            }
        }

        public virtual IQueryable<InvTrxDtl> GetInvDtlsById(int Id)
        {
            try
            {
                return _context.InvTrxDtls
                 .Include(i => i.InvItem)
                 .Include(i => i.InvTrxHdr)
                 .Include(i => i.InvUom)
                 .Where(i => i.Id == Id);
            }
            catch (Exception ex)
            {
                    _logger.LogError("ItemDtlsServices: Unable to GetInvDtlsById :" + ex.Message);
                    throw new Exception("ItemDtlsServices: Unable to GetInvDtlsById :" + ex.Message);
                
            }
        }

        public virtual async Task<InvTrxDtl> GetInvDtlsByIdAsync(int Id)
        {
            return await _context.InvTrxDtls.FindAsync(Id);

        }

        public virtual async Task<InvTrxDtl> GetInvDtlsByIdOnEdit(int Id)
        {
            var invTrx = await _context.InvTrxDtls
                .Include(i => i.InvItem)
                .ThenInclude(i => i.InvWarningLevels)
                .ThenInclude(i => i.InvWarningType)
                .Include(i => i.InvTrxHdr)
                .Include(i => i.InvUom)
                .FirstOrDefaultAsync(m => m.Id == Id);

            if (invTrx == null)
            {
                return new InvTrxDtl();
            }

            return invTrx;
        }

        public virtual IQueryable<InvTrxHdr> GetInvDtlsByStoreId(int storeId, int typeId)
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<InvTrxDtlOperator> GetInvTrxDtlOperators()
        {
            try
            {
                return _context.InvTrxDtlOperators;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemDtlsServices: Error on GetInvTrxDtlOperators :" + ex.Message);
                throw new Exception("ItemDtlsServices: Error on  GetInvTrxDtlOperators :" + ex.Message);

            }
        }

        public virtual bool InvTrxDtlsExists(int id)
        {
            return _context.InvTrxDtls.Any(e => e.Id == id);
        }

        public virtual async Task SaveChangesAsync()
        {
             await _context.SaveChangesAsync();
        }

        IQueryable<InvTrxDtl> IItemDtlsServices.GetInvDtlsByStoreId(int storeId, int typeId)
        {
            throw new NotImplementedException();
        }

        public virtual ReleasingItemDtlsCreateEditModel GetReleasingItemTrxDtlsModel_OnCreateOnGet(InvTrxDtl invTrxDtl, int hdrId, int invItemId)
        {
            //try
            //{

                int storeId = itemTrxServices.GetInvTrxStoreId((int)hdrId);
                var lotNoList = itemServices.GetLotNotItemList(invItemId, storeId);
                var availableItems = storeServices.GetAvailableItemsIdsByStore(storeId);

                var ItemDtlsCreateEditModel = new ReleasingItemDtlsCreateEditModel();

                if (invTrxDtl == null)
                {
                    invTrxDtl = new InvTrxDtl();
                    invTrxDtl.InvTrxHdrId = hdrId;
                    invTrxDtl.InvTrxDtlOperatorId = OPERATION_SUBTRACT;
                }

                ItemDtlsCreateEditModel.InvTrxDtl = invTrxDtl;

                ItemDtlsCreateEditModel.LotNo = new SelectList(lotNoList.Select(x => new {
                    Name = String.Format("{0} ", x.LotNo),
                    Value = x.LotNo
                }), "Value", "Name");

                ItemDtlsCreateEditModel.InvItems = new SelectList(itemServices.GetInStockedInvItemsSelectList(invItemId, availableItems)
                                        .Include(i => i.InvCategory)
                                        .Select(x => new {
                                            Name = String.Format("{0} - {1} - {2} {3}",
                                           x.Code, x.InvCategory.Description, x.Description, x.Remarks),
                                            Value = x.Id
                                        }), "Value", "Name", invItemId);

                ItemDtlsCreateEditModel.InvUoms = new SelectList(uomServices.GetUomSelectListByItemId(null), "Id", "uom");
                ItemDtlsCreateEditModel.InvTrxHdrs = new SelectList(itemTrxServices.GetInvTrxHdrs(), "Id", "Id", hdrId);
                ItemDtlsCreateEditModel.InvTrxDtlOperators = new SelectList(this.GetInvTrxDtlOperators(), "Id", "Description", OPERATION_SUBTRACT);
                ItemDtlsCreateEditModel.HrdId = (int)hdrId;
                ItemDtlsCreateEditModel.LotNoItems = lotNoList;
                ItemDtlsCreateEditModel.StoreId = storeId;
                ItemDtlsCreateEditModel.SelectedItem = " ";

                return ItemDtlsCreateEditModel; // return null
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError("ItemDtlsServices: Error on GetReleasingItemTrxDtlsModel_OnCreateOnGet :" + ex.Message);
            //    throw new Exception("ItemDtlsServices: Error on  GetReleasingItemTrxDtlsModel_OnCreateOnGet :" + ex.Message);
            //    // return new ReleasingCreateEditModel(); // return null

            //}
        }

        public ReceivingItemDtlsCreateEditModel GeReceivingItemDtlsCreateModel_OnCreateOnGet(InvTrxDtl invTrxDtl, int hdrId, int id)
        {
            try
            {
                var invTrxHeader = itemTrxServices.GetInvTrxHdrsById(hdrId).FirstOrDefault();

                if (invTrxHeader == null)
                {
                    return new ReceivingItemDtlsCreateEditModel();
                }
                
                var receivingItemDtls = new ReceivingItemDtlsCreateEditModel();
                
                receivingItemDtls.InvTrxDtl = new InvTrxDtl();
                receivingItemDtls.InvTrxDtl.InvTrxHdrId = hdrId;
                receivingItemDtls.InvTrxDtl.InvTrxDtlOperatorId = OPERATION_ADD;

                receivingItemDtls.InvItems = new SelectList(itemServices.GetInvItemsSelectList().Include(i => i.InvCategory)
                                        .Select(x => new
                                        {
                                            Name = String.Format("{0} - {1} - {2} {3}",
                                            x.Code, x.InvCategory.Description, x.Description, x.Remarks),
                                            Value = x.Id
                                        }), "Value", "Name", id);

                receivingItemDtls.InvUoms = new SelectList(uomServices.GetUomSelectListByItemId(receivingItemDtls.InvTrxDtl.InvItemId), "Id", "uom");
                receivingItemDtls.InvTrxHdrs = new SelectList(dbMaster.InvTrxHdrDb.GetInvTrxHdrs(), "Id", "Id", hdrId);
                receivingItemDtls.InvTrxDtlOperators = new SelectList(_context.InvTrxDtlOperators, "Id", "Description", OPERATION_ADD);
                receivingItemDtls.HrdId = hdrId;
                receivingItemDtls.StoreId = invTrxHeader.InvStoreId;

                return receivingItemDtls;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemDtlsServices: Error on GeReceivingItemDtlsCreateModel_OnCreateOnGet :" + ex.Message);
                throw new Exception("ItemDtlsServices: Error on  GeReceivingItemDtlsCreateModel_OnCreateOnGet :" + ex.Message);
            }
        }

        public ReceivingItemDtlsCreateEditModel GeReceivingItemDtlsEditModel_OnEditOnGet(InvTrxDtl invTrxDtl, int hdrId)
        {
            try
            {
                var invTrxHeader = itemTrxServices.GetInvTrxHdrsById(hdrId).FirstOrDefault();

                if (invTrxHeader == null)
                {
                    return new ReceivingItemDtlsCreateEditModel();
                }

                var receivingItemDtls = new ReceivingItemDtlsCreateEditModel();

                receivingItemDtls.InvTrxDtl = invTrxDtl;
                receivingItemDtls.InvTrxDtl.InvTrxHdrId = hdrId;

                receivingItemDtls.InvItems = new SelectList(itemServices.GetInvItemsSelectList().Include(i => i.InvCategory)
                                        .Select(x => new
                                        {
                                            Name = String.Format("{0} - {1} - {2} {3}",
                                            x.Code, x.InvCategory.Description, x.Description, x.Remarks),
                                            Value = x.Id
                                        }), "Value", "Name");

                receivingItemDtls.InvUoms = new SelectList(uomServices.GetUomSelectListByItemId(receivingItemDtls.InvTrxDtl.InvItemId), "Id", "uom");
                receivingItemDtls.InvTrxHdrs = new SelectList(dbMaster.InvTrxHdrDb.GetInvTrxHdrs(), "Id", "Id", hdrId);
                receivingItemDtls.InvTrxDtlOperators = new SelectList(_context.InvTrxDtlOperators, "Id", "Description", 1);
                receivingItemDtls.HrdId = hdrId;
                receivingItemDtls.StoreId = invTrxHeader.InvStoreId;

                return receivingItemDtls;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemDtlsServices: Error on GeReceivingItemDtlsCreateModel_OnCreateOnGet :" + ex.Message);
                throw new Exception("ItemDtlsServices: Error on  GeReceivingItemDtlsCreateModel_OnCreateOnGet :" + ex.Message);
            }
        }

        public ReceivingItemDtlsCreateEditModel GeReceivingItemDtlsEditModel_OnEditOnGet(InvTrxDtl invTrxDtl)
        {
            try
            {
                var invTrxHeader = itemTrxServices.GetInvTrxHdrsById(invTrxDtl.InvTrxHdrId).FirstOrDefault();

                if (invTrxHeader == null)
                {
                    return new ReceivingItemDtlsCreateEditModel();
                }

                var receivingItemDtls = new ReceivingItemDtlsCreateEditModel();
                receivingItemDtls.InvTrxDtl = invTrxDtl;

                receivingItemDtls.InvItems = new SelectList(itemServices.GetInvItemsSelectList().Include(i => i.InvCategory)
                                        .Select(x => new
                                        {
                                            Name = String.Format("{0} - {1} - {2} {3}",
                                            x.Code, x.InvCategory.Description, x.Description, x.Remarks),
                                            Value = x.Id
                                        }), "Value", "Name", receivingItemDtls.InvTrxDtl.InvItemId);

                receivingItemDtls.InvTrxHdrs = new SelectList(dbMaster.InvTrxHdrDb.GetInvTrxHdrs(), "Id", "Id");
                receivingItemDtls.InvUoms = new SelectList(uomServices.GetUomSelectListByItemId(receivingItemDtls.InvTrxDtl.InvItemId), "Id", "uom");
                receivingItemDtls.InvTrxDtlOperators = new SelectList(dbMaster.InvTrxDtlOperatorDb.GetOperators(), "Id", "Description");
                receivingItemDtls.HrdId = invTrxDtl.InvTrxHdrId;
                receivingItemDtls.StoreId = invTrxHeader.InvStoreId;

                return receivingItemDtls;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemDtlsServices: Error on GeReceivingItemDtlsCreateModel_OnCreateOnGet :" + ex.Message);
                throw new Exception("ItemDtlsServices: Error on  GeReceivingItemDtlsCreateModel_OnCreateOnGet :" + ex.Message);
            }
        }

        public async Task<TrxDetailsItemDetailsModel> GetTrxDetailsModel_OnDetailsAsync(int id)
        {

            try
            {
                var trxDetailsModel = new TrxDetailsItemDetailsModel();

                var trxDetails = await dbMaster.InvTrxDtlDb.GetInvTrxDtl().FirstOrDefaultAsync(m => m.Id == id);

                if (trxDetails == null)
                {
                    return trxDetailsModel;
                }

                trxDetailsModel.InvTrxDtl = trxDetails;
                trxDetailsModel.HrdId = trxDetailsModel.InvTrxDtl.InvTrxHdrId;

                return trxDetailsModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemDtlsServices: Error on GetTrxDetailsModel_OnDetailsAsync :" + ex.Message);
                throw new Exception("ItemDtlsServices: Error on  GetTrxDetailsModel_OnDetailsAsync :" + ex.Message);
            }

        }

        public async Task<TrxDetailsItemDeleteModel> GetTrxDetailsModel_OnDeleteAsync(int id)
        {
            try
            {
                var trxDeleteModel = new TrxDetailsItemDeleteModel();

                var trxDetails = await dbMaster.InvTrxDtlDb.GetInvTrxDtl().FirstOrDefaultAsync(m => m.Id == id);

                if (trxDetails == null)
                {
                    return trxDeleteModel;
                }

                trxDeleteModel.InvTrxDtl = trxDetails;
                trxDeleteModel.HrdId = trxDeleteModel.InvTrxDtl.InvTrxHdrId;

                return trxDeleteModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemDtlsServices: Error on GetTrxDetailsModel_OnDeleteAsync :" + ex.Message);
                throw new Exception("ItemDtlsServices: Error on  GetTrxDetailsModel_OnDeleteAsync :" + ex.Message);
            }
        }

        public TrxItemsCreateEditModel GeItemDtlsCreateModel_OnCreateOnGet(InvTrxDtl invTrxDtl, int hdrId)
        {
            //try
            //{
                var invTrxHeader = itemTrxServices.GetInvTrxHdrsById(hdrId).FirstOrDefault();

                if (invTrxHeader == null)
                {
                    return new TrxItemsCreateEditModel();
                }

                var trxItemDtls = new TrxItemsCreateEditModel();

                trxItemDtls.InvTrxDtl = new InvTrxDtl();
                trxItemDtls.InvTrxDtl.InvItemId = 2; // default
                trxItemDtls.InvTrxDtl.InvTrxHdrId = hdrId;
                trxItemDtls.InvTrxDtl.InvTrxDtlOperatorId = 1;

                trxItemDtls.InvItems = new SelectList(itemServices.GetInvItemsSelectList().Include(i => i.InvCategory)
                                        .Select(x => new
                                        {
                                            Name = String.Format("{0} - {1} - {2} {3}",
                                            x.Code, x.InvCategory.Description, x.Description, x.Remarks),
                                            Value = x.Id
                                        }), "Value", "Name");

                trxItemDtls.InvUoms = new SelectList(uomServices.GetUomSelectList(), "Id", "uom");
                trxItemDtls.InvTrxHdrs = new SelectList(dbMaster.InvTrxHdrDb.GetInvTrxHdrs(), "Id", "Id", hdrId);
                trxItemDtls.InvTrxDtlOperators = new SelectList(_context.InvTrxDtlOperators, "Id", "Description", 1);
                trxItemDtls.HrdId = hdrId;
                trxItemDtls.StoreId = invTrxHeader.InvStoreId;

                return trxItemDtls;
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError("ItemDtlsServices: Error on GeItemDtlsCreateModel_OnCreateOnGet :" + ex.Message);
            //    throw new Exception("ItemDtlsServices: Error on GeItemDtlsCreateModel_OnCreateOnGet :" + ex.Message);
            //}
        }

        public TrxItemsCreateEditModel GeItemDtlsEditModel_OnEditOnGet(InvTrxDtl invTrxDtl)
        {
            try
            {
                var invTrxHeader = itemTrxServices.GetInvTrxHdrsById(invTrxDtl.InvTrxHdrId).FirstOrDefault();

                if (invTrxHeader == null)
                {
                    return new TrxItemsCreateEditModel();
                }

                var receivingItemDtls = new TrxItemsCreateEditModel();
                receivingItemDtls.InvTrxDtl = invTrxDtl;

                receivingItemDtls.InvItems = new SelectList(itemServices.GetInvItemsSelectList().Include(i => i.InvCategory)
                                        .Select(x => new
                                        {
                                            Name = String.Format("{0} - {1} - {2} {3}",
                                            x.Code, x.InvCategory.Description, x.Description, x.Remarks),
                                            Value = x.Id
                                        }), "Value", "Name", receivingItemDtls.InvTrxDtl.InvItemId);

                receivingItemDtls.InvTrxHdrs = new SelectList(dbMaster.InvTrxHdrDb.GetInvTrxHdrs(), "Id", "Id");
                receivingItemDtls.InvUoms = new SelectList(uomServices.GetUomSelectListByItemId(receivingItemDtls.InvTrxDtl.InvItemId), "Id", "uom");
                receivingItemDtls.InvTrxDtlOperators = new SelectList(dbMaster.InvTrxDtlOperatorDb.GetOperators(), "Id", "Description");
                receivingItemDtls.HrdId = invTrxDtl.InvTrxHdrId;
                receivingItemDtls.StoreId = invTrxHeader.InvStoreId;

                return receivingItemDtls;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemDtlsServices: Error on GeReceivingItemDtlsCreateModel_OnCreateOnGet :" + ex.Message);
                throw new Exception("ItemDtlsServices: Error on  GeReceivingItemDtlsCreateModel_OnCreateOnGet :" + ex.Message);
            }
        }
    }
}
