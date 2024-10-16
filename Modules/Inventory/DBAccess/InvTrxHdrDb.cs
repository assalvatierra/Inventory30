﻿using CoreLib.Interfaces.DBAccess;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DBAccess
{
    public class InvTrxHdrDb : IInvTrxHdrDb
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public InvTrxHdrDb(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;

        }

        public IQueryable<InvTrxHdr> GetInvTrxHdrs()
        {
            return _context.InvTrxHdrs;
        }


        public InvTrxHdr GetInvTrxHdrsById(int id)
        {
            var invtrxHdr = _context.InvTrxHdrs.Find(id);

            if (invtrxHdr == null)
            {
                return new InvTrxHdr();
            }

            return invtrxHdr;
        }

        public async Task<IList<InvTrxHdr>> GetInvTrxHdrsByTypeIdAndStoreId(int typeId, int storeId)
        {
            var invtrxHdr = await _context.InvTrxHdrs
                .Include(i => i.InvStore)
                .Include(i => i.InvTrxHdrStatu)
                .Include(i => i.InvTrxType)
                .Include(i => i.InvTrxDtls)
                    .ThenInclude(i => i.InvItem)
                    .ThenInclude(i => i.InvUom)
                .Include(i=>i.InvTrxDtls)
                    .ThenInclude(i => i.InvTrxDtlOperator)
                .Where(i => i.InvTrxTypeId == typeId &&
                              i.InvStoreId == storeId)
                .ToListAsync();

            if (invtrxHdr == null)
            {
                return null;
            }

            return invtrxHdr;
        }
    }
}
