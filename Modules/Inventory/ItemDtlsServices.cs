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
        private readonly DBMasterService dBMaster;
        private readonly IUomServices uomServices;
        private readonly IItemServices itemServices;

        private readonly int TYPE_RELEASING = 2;

        public ItemDtlsServices(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;

            dBMaster = new DBMasterService(_context, _logger);
            uomServices = new UomServices(_context);
            itemServices = new ItemServices(_context);
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

        public virtual ReleasingCreateEditModel GetReleasingItemTrxDtlsModel_OnCreateOnGet(InvTrxDtl invTrxDtl, int storeId, List<InvTrxHdr> invTrxHdrs, IList<InvItem> invItems, IList<ItemLotNoSelect> itemLotNoSelects, IList<int> availableItems, IList<InvUom> invUoms)
        {
            try
            {
                return new ReleasingCreateEditModel(); // return null
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemDtlsServices: Error on GetReleasingItemTrxDtlsModel_OnCreateOnGet :" + ex.Message);
                throw new Exception("ItemDtlsServices: Error on  GetReleasingItemTrxDtlsModel_OnCreateOnGet :" + ex.Message);
                // return new ReleasingCreateEditModel(); // return null

            }
        }

        public ReceivingItemDtlsCreateEditModel GeReceivingItemDtlsCreateModel_OnCreateOnGet(InvTrxDtl invTrxDtl, int hdrId)
        {
            try
            {
                var invTrxHeader = _context.InvTrxHdrs.Find(hdrId);
                
                var receivingItemDtls = new ReceivingItemDtlsCreateEditModel();
                
                receivingItemDtls.InvTrxDtl = new InvTrxDtl();
                receivingItemDtls.InvTrxDtl.InvItemId = 2;
                receivingItemDtls.InvTrxDtl.InvTrxHdrId = hdrId;

                receivingItemDtls.InvItems = new SelectList(itemServices.GetInvItemsSelectList().Include(i => i.InvCategory)
                                        .Select(x => new
                                        {
                                            Name = String.Format("{0} - {1} - {2} {3}",
                                            x.Code, x.InvCategory.Description, x.Description, x.Remarks),
                                            Value = x.Id
                                        }), "Value", "Name");

                receivingItemDtls.InvUoms = new SelectList(uomServices.GetUomSelectListByItemId(receivingItemDtls.InvTrxDtl.InvItemId), "Id", "uom");
                receivingItemDtls.InvTrxHdrs = new SelectList(_context.InvTrxHdrs, "Id", "Id", hdrId);
                receivingItemDtls.InvTrxDtlOperators = new SelectList(_context.InvTrxDtlOperators, "Id", "Description", 1);
                receivingItemDtls.HrdId = hdrId;
                receivingItemDtls.StoreId = invTrxHeader.InvStoreId;

                return receivingItemDtls;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemDtlsServices: Error on GetReleasingItemTrxDtlsModel_OnCreateOnGet :" + ex.Message);
                throw new Exception("ItemDtlsServices: Error on  GetReleasingItemTrxDtlsModel_OnCreateOnGet :" + ex.Message);
            }
        }
    }
}
