using CoreLib.DTO.PurchaseOrder;
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
            throw new NotImplementedException();
        }

        public void DeleteInvPoHdrs(InvPoHdr InvPoHdr)
        {
            throw new NotImplementedException();
        }

        public void EditInvPoHdrs(InvPoHdr InvPoHdr)
        {
            throw new NotImplementedException();
        }

        public Task<InvPoHdr> GetInvPoHdrsbyIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<InvPoHdr>> GetInvPoHdrsListAsync(int storeId)
        {

            return await _context.InvPoHdrs
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
                _logger.LogError("ItemTrxServices: Unable to SortInvPOHdrList_Status :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to SortInvPOHdrList_Status :" + ex.Message);

            }
        }

        public bool InvTrxDtlsExists(int id)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public InvPOHdrCreateModel GetInvPOHdrModel_OnCreate(InvPOHdrCreateModel InvPoHdr, int storeId, string User)
        {
            try
            {

                InvPoHdr = new InvPOHdrCreateModel();
                InvPoHdr.InvPoHdr = new InvPoHdr();

                InvPoHdr.InvPoHdrStatusId = new SelectList(_context.InvPoHdrStatus, "Id", "Status");
                InvPoHdr.InvStoreId = new SelectList(_context.InvStores, "Id", "StoreName", storeId);
                InvPoHdr.InvSupplierId = new SelectList(_context.InvSuppliers, "Id", "Name");
                InvPoHdr.UserId = User;
                InvPoHdr.StoreId = storeId;

                return InvPoHdr;

            }
            catch (Exception ex)
            {
                _logger.LogError("ItemTrxServices: Unable to SortInvPOHdrList_Status :" + ex.Message);
                throw new Exception("ItemTrxServices: Unable to SortInvPOHdrList_Status :" + ex.Message);

            }
        }
    }
}
