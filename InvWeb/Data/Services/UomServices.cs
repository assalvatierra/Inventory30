
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDBSchema.Models;
using WebDBSchema.Models.Items;
using InvWeb.Data.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

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

        public SelectList GetUomSelectList()
        {
            try
            {
                return new SelectList(_context.InvUoms
                    .Select(x => new
                        {
                            Name = x.uom,
                            Value = x.Id
                        }), "Value", "Name");
            }
            catch
            {
                return new SelectList(null);
            }
        }

        public SelectList GetUomSelectListByItemId(int? itemId)
        {
            try
            {
                if (itemId == null)
                {
                    return new SelectList(null);
                    //return GetUomSelectList();
                }

                var item = _context.InvItems.Find(itemId);

                if (item == null)
                {
                    return new SelectList(null);
                    //return GetUomSelectList();
                }

                var item_BaseUom = item.InvUomId;

                List<int> UomConversionList = _context.InvUomConversions
                    .Where(uom => uom.InvUomId_base == item_BaseUom)
                    .Select(c => c.InvUomId_into).ToList();

                if (UomConversionList == null)
                {
                    return new SelectList(_context.InvUoms
                        .Where(i => item_BaseUom == i.Id)
                        .Select(x => new {
                            Name = x.uom,
                            Value = x.Id
                        }), "Value", "Name");
                }

                return new SelectList(_context.InvUoms
                    .Where(i => UomConversionList.Contains(i.Id) ||
                                item_BaseUom == i.Id)
                    .Select(x => new {
                            Name = x.uom,
                            Value = x.Id
                        }), "Value", "Name");
            }
            catch (Exception ex)
            {
                throw ex;
                return new SelectList(null);
            }
        }
    }
}
