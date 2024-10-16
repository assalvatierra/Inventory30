﻿using CoreLib.DTO.PurchaseOrder;
using CoreLib.Interfaces;
using CoreLib.Inventory.Interfaces;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using Inventory.DBAccess;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Inventory
{
    public class InvPOHdrServices : IInvPOHdrServices
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly DBMasterService dbMaster;

        public InvPOHdrServices(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;

            dbMaster = new DBMasterService(_context, _logger);
        }

        public void CreateInvPoHdrs(InvPoHdr InvPoHdr)
        {
            try
            {
                dbMaster.InvPOHdrDb.CreateInvPOHdr(InvPoHdr);
            }
            catch (Exception ex)
            {
                _logger.LogError("InvPOHdrServices: Unable to CreateInvPoHdrs :" + ex.Message);
                throw new Exception("InvPOHdrServices: Unable to CreateInvPoHdrs :" + ex.Message);

            }

        }

        public void DeleteInvPoHdrs(InvPoHdr InvPoHdr)
        {
            dbMaster.InvPOHdrDb.DeleteInvPOHdr(InvPoHdr);
        }

        public void EditInvPoHdrs(InvPoHdr InvPoHdr)
        {
            try
            {
                dbMaster.InvPOHdrDb.EditInvPOHdr(InvPoHdr);
            }
            catch (Exception ex)
            {
                _logger.LogError("InvPOHdrServices: Unable to EditInvPoHdrs :" + ex.Message);
                throw new Exception("InvPOHdrServices: Unable to EditInvPoHdrs :" + ex.Message);

            }
        }

        public async Task<InvPoHdr> GetInvPoHdrsbyIdAsync(int id)
        {
            var InvPOHdr = await dbMaster.InvPOHdrDb.GetInvPOHdrs()
                 .Include(i => i.InvPoHdrStatu)
                 .Include(i => i.InvStore)
                 .Include(i => i.InvSupplier).FirstOrDefaultAsync(m => m.Id == id);

            return InvPOHdr;
        }

        public async Task<IEnumerable<InvPoHdr>> GetInvPoHdrsListAsync(int storeId)
        {

            return await dbMaster.InvPOHdrDb.GetInvPOHdrs()
                .Include(i => i.InvPoHdrStatu)
                .Include(i => i.InvStore)
                .Include(i => i.InvSupplier)
                .Include(i => i.InvPoItems)
                    .ThenInclude(i => i.InvItem)
                    .ThenInclude(i => i.InvUom)
                  .Where(i => i.InvStoreId == storeId)
                .ToListAsync();

        }

        public async Task<InvPOHdrModel> GetInvPOHdrModel_OnIndex(IList<InvPoHdr> InvPoHdrs, int storeId, string status,bool IsUserAdmin)
        {
            try
            {

                InvPOHdrModel invPOHdr = new InvPOHdrModel();

                InvPoHdrs = (IList<InvPoHdr>)await this.GetInvPoHdrsListAsync(storeId);

                //sort status
                invPOHdr.InvPoHdrs = SortInvPOHdrList_Status(InvPoHdrs, status);

                invPOHdr.StoreId = storeId;
                invPOHdr.Status = status;
                invPOHdr.IsAdmin = IsUserAdmin;

                return invPOHdr;

            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to GetInvPOHdrModel_OnIndex :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to GetInvPOHdrModel_OnIndex :" + ex.Message);

            }

        }

        private IList<InvPoHdr> SortInvPOHdrList_Status(IList<InvPoHdr> invPoHdr, string status)
        {

            try
            {
                if (!String.IsNullOrWhiteSpace(status))
                {
                    invPoHdr = status switch
                    {
                        "PENDING" => invPoHdr.Where(i => i.InvPoHdrStatusId == 1).ToList(),
                        "ACCEPTED" => invPoHdr.Where(i => i.InvPoHdrStatusId == 2).ToList(),
                        "ALL" => invPoHdr.ToList(),
                        _ => invPoHdr.Where(i => i.InvPoHdrStatusId == 1).ToList(),
                    };
                }

                return invPoHdr;
          
            }
            catch (Exception ex)
            {
                _logger.LogError("InvPOHdrServices: Unable to SortInvPOHdrList_Status :" + ex.Message);
                throw new Exception("InvPOHdrServices: Unable to SortInvPOHdrList_Status :" + ex.Message);

            }
        }

        public bool InvTrxDtlsExists(int id)
        {
            return _context.InvPoHdrs.Any(e => e.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public InvPOHdrCreateEditModel GetInvPOHdrModel_OnCreate(InvPOHdrCreateEditModel InvPoHdr, int storeId, string User)
        {
            try
            {

                InvPoHdr = new InvPOHdrCreateEditModel();
                InvPoHdr.InvPoHdr = new InvPoHdr();

                InvPoHdr.InvPoHdrStatusId = new SelectList(dbMaster.InvTrxHdrStatusDb.GetInvTrxHdrStatus(), "Id", "Status");
                InvPoHdr.InvStoreId = new SelectList(dbMaster.StoreDb.GetStoreList(), "Id", "StoreName", storeId);
                InvPoHdr.InvSupplierId = new SelectList(dbMaster.InvSupplierDb.GetInvSuppliers(), "Id", "Name");
                InvPoHdr.UserId = User;
                InvPoHdr.StoreId = storeId;

                InvPoHdr.InvPoHdr.InvStoreId = storeId;
                InvPoHdr.InvPoHdr.UserId = User;

                return InvPoHdr;

            }
            catch (Exception ex)
            {
                _logger.LogError("InvPOHdrServices: Unable to GetInvPOHdrModel_OnCreate :" + ex.Message);
                throw new Exception("InvPOHdrServices: Unable to GetInvPOHdrModel_OnCreate :" + ex.Message);

            }
        }

        public InvPOHdrCreateEditModel GetInvPOHdrModel_OnEdit(InvPOHdrCreateEditModel InvPoHdr)
        {
            try
            {

                InvPoHdr.InvPoHdrStatusId = new SelectList(dbMaster.InvTrxHdrStatusDb.GetInvTrxHdrStatus(), "Id", "Status");
                InvPoHdr.InvStoreId = new SelectList(dbMaster.StoreDb.GetStoreList(), "Id", "StoreName");
                InvPoHdr.InvSupplierId = new SelectList(dbMaster.InvSupplierDb.GetInvSuppliers(), "Id", "Name");
                InvPoHdr.UserId = InvPoHdr.UserId;
                InvPoHdr.StoreId = InvPoHdr.StoreId;

                return InvPoHdr;

            }
            catch (Exception ex)
            {
                _logger.LogError("InvPOHdrServices: Unable to GetInvPOHdrModel_OnCreate :" + ex.Message);
                throw new Exception("InvPOHdrServices: Unable to GetInvPOHdrModel_OnCreate :" + ex.Message);

            }
        }

        public void RemoveInvPOHdrDeleteModel(InvPOHdrDeleteModel InvPoHdrDelete)
        {
            try
            {
                var itemList = dbMaster.InvPOItemDb.GetInvPoItemsByHdrId(InvPoHdrDelete.InvPoHdr.Id);
                _context.InvPoItems.RemoveRange(itemList);

                DeleteInvPoHdrs(InvPoHdrDelete.InvPoHdr);
            }
            catch (Exception ex)
            {
                _logger.LogError("InvPOHdrServices: Unable to GetInvPOHdrModel_OnCreate :" + ex.Message);
                throw new Exception("InvPOHdrServices: Unable to GetInvPOHdrModel_OnCreate :" + ex.Message);

            }
        }

        public async Task<InvPoHdr> InvPOHdrDelete_FindByIdAsync(int id)
        {
            return await dbMaster.InvPOHdrDb.FindInvPOHdrByIdAsync(id);
        }

        public async Task<InvPOHdrDetailsModel> GetInvPOHdrModel_OnDetails(InvPOHdrDetailsModel InvPOHdrDetails, int invPOHdrId , string status, bool IsUserAdmin)
        {
            try
            {

                if (InvPOHdrDetails == null)
                {
                    InvPOHdrDetails = new InvPOHdrDetailsModel();
                }

                 var invPoHeader = await dbMaster.InvPOHdrDb.GetInvPOHdrs()
                    .Include(i => i.InvPoHdrStatu)
                    .Include(i => i.InvStore)
                    .Include(i => i.InvSupplier).FirstOrDefaultAsync(m => m.Id == invPOHdrId);

                if (invPoHeader == null)
                {
                    return InvPOHdrDetails;
                }

                InvPOHdrDetails.InvPoHdr = invPoHeader;

                InvPOHdrDetails.InvPoItems = await dbMaster.InvPOItemDb.GetInvPoItems()
                    .Include(i => i.InvItem)
                    .Include(i => i.InvPoHdr)
                    .Include(i => i.InvUom)
                    .Where(i => i.InvPoHdrId == invPOHdrId)
                    .ToListAsync();


                InvPOHdrDetails.StoreId = InvPOHdrDetails.InvPoHdr.InvStoreId;

                return InvPOHdrDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError("InvPOHdrServices: Unable to GetInvPOHdrModel_OnCreate :" + ex.Message);
                throw new Exception("InvPOHdrServices: Unable to GetInvPOHdrModel_OnCreate :" + ex.Message);

            }
        }
    }
}
