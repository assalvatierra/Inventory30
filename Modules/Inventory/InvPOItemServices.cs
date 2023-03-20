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
    public class InvPOItemServices : IInvPOItemServices
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly DBMasterService dbMaster;
        private readonly IUomServices uomServices;

        public InvPOItemServices(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;

            dbMaster = new DBMasterService(_context, _logger);
            uomServices = new UomServices(_context);
        }

        public void CreateInvPoItem(InvPoItem invPoItem)
        {
            try
            {
                dbMaster.InvPOItemDb.CreateInvPoItem(invPoItem);
            }
            catch (Exception ex)
            {
                _logger.LogError("InvPOHdrServices: Unable to CreateInvPoItem :" + ex.Message);
                throw new Exception("InvPOHdrServices: Unable to CreateInvPoItem :" + ex.Message);

            }
        }

        public async Task DeleteInvPoItem(int id)
        {
            try
            {

                var invPoItem = await dbMaster.InvPOItemDb.GetInvPoItemAsync(id);

                if (invPoItem != null)
                {
                    dbMaster.InvPOItemDb.DeleteInvPoItem(invPoItem);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("InvPOHdrServices: Unable to DeleteInvPoItem :" + ex.Message);
                throw new Exception("InvPOHdrServices: Unable to DeleteInvPoItem :" + ex.Message);

            }
        }


        public void EditInvPoItem(InvPoItem invPoItem)
        {
            try
            {
                dbMaster.InvPOItemDb.EditInvPoItem(invPoItem);
            }
            catch (Exception ex)
            {
                _logger.LogError("InvPOHdrServices: Unable to EditInvPoItem :" + ex.Message);
                throw new Exception("InvPOHdrServices: Unable to EditInvPoItem :" + ex.Message);

            }
        }

        public async Task<InvPoItem> GetInvPoItemById(int id)
        {
            return await dbMaster.InvPOItemDb.GetInvPoItem(id);
        }

        public InvPOItemCreateEditModel GetInvPOItemModel_OnCreate(InvPOItemCreateEditModel InvPOItemCreate, int hdrId)
        {
            try
            {
                if (InvPOItemCreate == null)
                {
                    InvPOItemCreate = new InvPOItemCreateEditModel();
                }

                //create defaults
                InvPOItemCreate.InvPoItem = new InvPoItem();
                InvPOItemCreate.InvPoItem.InvPoHdrId = hdrId;
                InvPOItemCreate.InvPoItem.InvItemId = 1;

                InvPOItemCreate.InvItemList = new SelectList(_context.InvItems, "Id", "Description");
                InvPOItemCreate.InvPoHdrList = new SelectList(_context.InvPoHdrs, "Id", "Id", hdrId);
                InvPOItemCreate.InvUomList = new SelectList(uomServices.GetUomSelectListByItemId(InvPOItemCreate.InvPoItem.InvItemId), "Id", "uom");
                InvPOItemCreate.HdrId = hdrId;


                return InvPOItemCreate;
            }
            catch (Exception ex)
            {

                _logger.LogError("InvPOHdrServices: Unable to GetInvPOItemModel_OnCreate :" + ex.Message);
                throw new Exception("InvPOHdrServices: Unable to GetInvPOItemModel_OnCreate :" + ex.Message);
            }
        }

        public async Task<InvPOItemDelete> GetInvPOItemModel_OnDelete(InvPOItemDelete invPOItemDelete, int id)
        {
            try
            {
                if (invPOItemDelete == null)
                {
                    invPOItemDelete = new InvPOItemDelete();
                }

                var _tempItemDelete = await dbMaster.InvPOItemDb.GetInvPoItem(id);

                if (_tempItemDelete == null)
                {
                    return invPOItemDelete;
                }

                invPOItemDelete.InvPoItem = _tempItemDelete;
                invPOItemDelete.InvHdrId = _tempItemDelete.InvPoHdrId;

                return invPOItemDelete;

            }
            catch (Exception ex)
            {

                _logger.LogError("InvPOHdrServices: Unable to GetInvPOItemModel_OnDelete :" + ex.Message);
                throw new Exception("InvPOHdrServices: Unable to GetInvPOItemModel_OnDelete :" + ex.Message);
            }
        }

        public async Task<InvPOItemCreateEditModel> GetInvPOItemModel_OnEdit(InvPOItemCreateEditModel InvPOItemEdit, int id)
        {
            try
            {

                if (InvPOItemEdit == null)
                {
                    InvPOItemEdit = new InvPOItemCreateEditModel();
                }

                var invPoItem = await dbMaster.InvPOItemDb.GetInvPoItem(id);

                if (invPoItem == null)
                {
                    return InvPOItemEdit;
                }

                InvPOItemEdit.InvPoItem = invPoItem;

                //create defaults
                InvPOItemEdit.InvItemList = new SelectList(_context.InvItems, "Id", "Description");
                InvPOItemEdit.InvPoHdrList = new SelectList(_context.InvPoHdrs, "Id", "Id", InvPOItemEdit.InvPoItem.InvPoHdrId);
                InvPOItemEdit.InvUomList = new SelectList(uomServices.GetUomSelectListByItemId(InvPOItemEdit.InvPoItem.InvItemId), "Id", "uom");
              
                return InvPOItemEdit;
            }
            catch (Exception ex)
            {

                _logger.LogError("InvPOHdrServices: Unable to GetInvPOItemModel_OnCreate :" + ex.Message);
                throw new Exception("InvPOHdrServices: Unable to GetInvPOItemModel_OnCreate :" + ex.Message);
            }
        }

        public bool InvPOItemExists(int id)
        {
            return _context.InvPoItems.Any(e => e.Id == id);
        }

        public async Task SaveChangesAsync()
        {
             await _context.SaveChangesAsync();
        }

    }
}
