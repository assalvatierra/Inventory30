using CoreLib.Interfaces.DBAccess;
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
    public class InvTrxDtlDb : IInvTrxDtlDb
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public InvTrxDtlDb(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;

        }

        public IQueryable<InvTrxDtl> GetInvTrxDtl()
        {
           return _context.InvTrxDtls
                .Include(i => i.InvItem)
                .Include(i => i.InvTrxHdr)
                .Include(i => i.InvUom);
        }
    }
}
