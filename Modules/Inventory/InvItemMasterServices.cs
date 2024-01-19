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

        public void CreateInvItemMaster(InvItemMaster InvItemMaster)
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
           
                if (invDtlsId != 0 && invItemMasterId != 0)
                {
                    InvTrxDtlxItemMaster invTrxDtlxItemMasters = new InvTrxDtlxItemMaster();
                    invTrxDtlxItemMasters.InvTrxDtlId = invDtlsId;
                    invTrxDtlxItemMasters.InvItemMasterId = invItemMasterId;

                    _context.InvTrxDtlxItemMasters.Add(invTrxDtlxItemMasters);
                    await _context.SaveChangesAsync();

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


        public bool InvItemMasterExists(int id)
        {
            return _context.InvItemMasters.Any(e => e.Id == id);
        }

        public async Task SaveChangesAsync()
        {
             await _context.SaveChangesAsync();
        }

    }
}
