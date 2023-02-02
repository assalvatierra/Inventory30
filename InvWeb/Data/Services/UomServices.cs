
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLib.Inventory.Models;
using CoreLib.Inventory.Models.Items;
using InvWeb.Data.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using InvWeb.Data.Models;

namespace InvWeb.Data.Services
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

        public int GetConverted_ItemCount_ByDefaultUom(int itemId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InvUom> GetUomSelectList()
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

        public IEnumerable<InvUom> GetUomSelectListByItemId(int? itemId)
        {
            try
            {
                if (itemId == null)
                {
                    return new List<InvUom>();
                    //return GetUomSelectList();
                }

                var item = _context.InvItems.Find(itemId);

                if (item == null)
                {
                    return new List<InvUom>();
                    //return GetUomSelectList();
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
                return new List<InvUom>();
            }
        }


        public IEnumerable<InvUom> GetUomListByItemId(int? itemId)
        {

            throw new NotImplementedException();
        }

        public async Task<List<UomsApiModel.ItemOumList>> GetUomListByItemIdAsync(int? itemId)
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
                    UomList = await _context.InvUoms
                        .Where(i => item_BaseUom == i.Id)
                        .ToListAsync();
                }

                UomList = await _context.InvUoms
                    .Where(i => UomConversionList.Contains(i.Id) ||
                                item_BaseUom == i.Id)
                    .ToListAsync();

                return   UomList.Select(uom => 
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
