using CoreLib.Interfaces.DBAccess;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
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
            var invHdr = _context.InvTrxHdrs.Find(id);

            if (invHdr == null)
            {
                return null;
            }

            return invHdr;
        }
    }
}
