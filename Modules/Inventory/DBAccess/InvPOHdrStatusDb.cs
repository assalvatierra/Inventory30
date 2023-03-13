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
    public class InvPOHdrStatusDb : IInvPOHdrStatusDb
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public InvPOHdrStatusDb(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public IQueryable<InvPoHdrStatus> GetInvPoHdrStatus()
        {
            return _context.InvPoHdrStatus;
        }



    }
}
