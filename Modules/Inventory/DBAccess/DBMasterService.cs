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
    public class DBMasterService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public IStoreDb StoreDb;
        public IInvTrxHdrStatusDb InvTrxHdrStatusDb;


        public DBMasterService(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;

            StoreDb = new StoreDb(_context, _logger);
            InvTrxHdrStatusDb = new InvTrxHdrStatusDb(_context, _logger);

        }

        public IStoreDb GetStoreDb() {  return StoreDb; }
        public IInvTrxHdrStatusDb GetInvTrxHdrStatusDb() { return InvTrxHdrStatusDb; }

    }
}
