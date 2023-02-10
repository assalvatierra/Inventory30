
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLib.Inventory.Models;
using CoreLib.Inventory.Interfaces;
using Microsoft.Extensions.Logging;
using CoreLib.Inventory.Interfaces;
using CoreLib.Models.API;
using CoreLib.Models.Inventory;

namespace Modules.Inventory
{
    public class UomServices : IUomServices
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public UomServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public UomServices(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;

        }

        public virtual int GetConverted_ItemCount_ByDefaultUom(int itemId)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<InvUom> GetUomSelectList()
        {
            try
            {
                return _context.InvUoms.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("UomServices: Unable to GetUomSelectList " + ex.Message);
                return new List<InvUom>();
            }
        }

        public virtual IEnumerable<InvUom> GetUomSelectListByItemId(int? itemId)
        {
            try
            {
                if (itemId == null)
                {
                    //return default list
                    return GetUomSelectList();
                }

                var item = _context.InvItems.Find(itemId);

                if (item == null)
                {
                    //return new List<InvUom>();
                    return GetUomSelectList();
                }

                var item_BaseUom = item.InvUomId;

                List<int> UomConversionList = _context.InvUomConversions
                    .Where(uom => uom.InvUomId_base == item_BaseUom)
                    .Select(c => c.InvUomId_into).ToList();

                if (UomConversionList == null)
                {
                    return _context.InvUoms
                        .Where(i => item_BaseUom == i.Id)
                        .ToList();
                }

                return _context.InvUoms
                    .Where(i => UomConversionList.Contains(i.Id) ||
                                item_BaseUom == i.Id)
                    .ToList();
            }
            catch (Exception ex)
            {
                //throw ex;
                _logger.LogError("UomServices: Unable to GetUomSelectListByItemId " + ex.Message);
                return GetUomSelectList();
            }
        }

        public virtual IEnumerable<UomsApiModel.ItemOumList> GetItemUomListByItemId(int? itemId)
        {
            try
            {
                if (itemId == null)
                {
                    return new List<UomsApiModel.ItemOumList>(null);
                }

                var item = _context.InvItems.Find(itemId);

                if (item == null)
                {
                    return new List<UomsApiModel.ItemOumList>(null);
                }

                var UomList = new List<InvUom>();
                var item_BaseUom = item.InvUomId;

                IEnumerable<int> UomConversionList = _context.InvUomConversions
                    .Where(uom => uom.InvUomId_base == item_BaseUom)
                    .Select(c => c.InvUomId_into).ToList();

                if (UomConversionList == null)
                {
                    UomList = _context.InvUoms
                        .Where(i => item_BaseUom == i.Id)
                        .ToList();
                }

                UomList = _context.InvUoms
                    .Where(i => UomConversionList.Contains(i.Id) ||
                                item_BaseUom == i.Id)
                    .ToList();

                return UomList.Select(uom => 
                         new UomsApiModel.ItemOumList { Id = uom.Id, uom = uom.uom })
                         .ToList();

            }
            catch (Exception ex)
            {
                //throw ex;
                _logger.LogError("UomServices: Unable to GetUomListByItemIdAsync " + ex.Message);
                return new List<UomsApiModel.ItemOumList>(null);
            }
        }
    }
}
