using CoreLib.DTO.PurchaseOrder;
using CoreLib.Interfaces;
using CoreLib.Inventory.Interfaces;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using Inventory.DBAccess;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory
{
    public class InvItemMasterServices : IInvItemMasterServices
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly DBMasterService dbMaster;
        private readonly IUomServices uomServices;

        public InvItemMasterServices(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;

            dbMaster = new DBMasterService(_context, _logger);
            uomServices = new UomServices(_context);
        }

        public async Task CreateInvItemMaster(InvItemMaster InvItemMaster)
        {
            try
            {
                dbMaster.InvItemMasterDb.CreateInvItemMaster(InvItemMaster);
            }
            catch (Exception ex)
            {
                _logger.LogError("InvPOHdrServices: Unable to CreateInvItemMaster :" + ex.Message);
                throw new Exception("InvPOHdrServices: Unable to CreateInvItemMaster :" + ex.Message);

            }
        }

        public async Task CreateItemMasterInvDtlsLink(int invItemMasterId, int invDtlsId)
        {

            try
            {
            
                if (invDtlsId != 0 && invItemMasterId != 0)
                {
                    InvTrxDtlxItemMaster invTrxDtlxItemMasters = new InvTrxDtlxItemMaster();
                    invTrxDtlxItemMasters.InvTrxDtlId = invDtlsId;
                    invTrxDtlxItemMasters.InvItemMasterId = invItemMasterId;

                    _context.InvTrxDtlxItemMasters.Add(invTrxDtlxItemMasters);
                     await _context.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                _logger.LogError("InvPOHdrServices: Unable to CreateInvItemMaster :" + ex.Message);
                throw new Exception("InvPOHdrServices: Unable to CreateInvItemMaster :" + ex.Message);

            }

        }


        public async Task DeleteInvItemMaster(int id)
        {
            try
            {

                var InvItemMaster = await dbMaster.InvItemMasterDb.GetInvItemMasterAsync(id);

                if (InvItemMaster != null)
                {
                    dbMaster.InvItemMasterDb.DeleteInvItemMaster(InvItemMaster);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("InvPOHdrServices: Unable to DeleteInvItemMaster :" + ex.Message);
                throw new Exception("InvPOHdrServices: Unable to DeleteInvItemMaster :" + ex.Message);

            }
        }



        public async Task DeleteInvItemMasterLink(int id)
        {
            try
            {

                var InvItemMasterLink = _context.InvTrxDtlxItemMasters.Find(id);

                if (InvItemMasterLink != null)
                {
                    _context.InvTrxDtlxItemMasters.Remove(InvItemMasterLink);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("InvPOHdrServices: Unable to DeleteInvItemMasterLink :" + ex.Message);
                throw new Exception("InvPOHdrServices: Unable to DeleteInvItemMasterLink :" + ex.Message);

            }
        }


        public void EditInvItemMaster(InvItemMaster InvItemMaster)
        {
            try
            {
                dbMaster.InvItemMasterDb.EditInvItemMaster(InvItemMaster);
            }
            catch (Exception ex)
            {
                _logger.LogError("InvPOHdrServices: Unable to EditInvItemMaster :" + ex.Message);
                throw new Exception("InvPOHdrServices: Unable to EditInvItemMaster :" + ex.Message);

            }
        }

        public async Task<InvItemMaster> GetInvItemMasterById(int id)
        {
            return await dbMaster.InvItemMasterDb.GetInvItemMaster(id);
        }


        public async Task<List<InvItemMaster>> GetInvItemMasters_Received_ById(int invItemId)
        {
            try
            {
               return await _context.InvItemMasters
                         .Where(c => c.InvItemId == invItemId && c.InvTrxDtlxItemMasters.Count > 0 
                         && c.InvTrxDtlxItemMasters.FirstOrDefault().InvTrxDtl.InvTrxHdr.InvTrxTypeId == (int)OPERATION_TYPE.RECEIVING
                         && (c.InvTrxDtlxItemMasters.FirstOrDefault().InvTrxDtl.InvTrxHdr.InvTrxHdrStatusId == (int)HdrStatus.APPROVED
                         || c.InvTrxDtlxItemMasters.FirstOrDefault().InvTrxDtl.InvTrxHdr.InvTrxHdrStatusId == (int)HdrStatus.CLOSED))
                     .Include(c => c.InvItemBrand)
                     .Include(c => c.InvItemOrigin)
                     .Include(c => c.InvStoreArea)
                     .Include(c => c.InvTrxDtlxItemMasters)
                         .ThenInclude(c => c.InvTrxDtl)
                     .Include(c => c.InvTrxDtlxItemMasters)
                         .ThenInclude(c => c.InvTrxDtl)
                         .ThenInclude(c => c.InvTrxDtlOperator)
                     .Include(c => c.InvTrxDtlxItemMasters)
                         .ThenInclude(c => c.InvTrxDtl)
                         .ThenInclude(c => c.InvTrxHdr.InvTrxHdrStatu)
                     .Include(c => c.InvItem)
                     .Include(c => c.InvItem)
                         .ThenInclude(c => c.InvCategory)
                     .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("InvPOHdrServices: Unable to EditInvItemMaster :" + ex.Message);
                throw new Exception("InvPOHdrServices: Unable to EditInvItemMaster :" + ex.Message);

            }
        }



        public List<InvTrxDtlxItemMaster> GetInvTrxDtlxItemMaster_byId(int InvItemMasterId)
        {
            return _context.InvTrxDtlxItemMasters.Where(c => c.InvItemMasterId == InvItemMasterId)
                .Include(c => c.InvTrxDtl)
                .Include(c => c.InvTrxDtl.InvTrxDtlOperator)
                .ToList();
        }

        public bool InvItemMasterExists(int id)
        {
            return _context.InvItemMasters.Any(e => e.Id == id);
        }

        public async Task SaveChangesAsync()
        {
             await _context.SaveChangesAsync();
        }


        private enum HdrStatus : int
        {
            REQUEST = 1,
            APPROVED = 2,
            CLOSED = 3,
            CANCELLED = 4,
            VERIFIED = 5,
            ACCEPTED = 6
        }
        private enum OPERATION_TYPE : int
        {
            RECEIVING = 1,
            RELEASING = 2,
            ADJUSTMENT = 3
        }

    }
}
