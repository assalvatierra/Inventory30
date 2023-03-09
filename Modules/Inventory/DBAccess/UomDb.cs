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
    public class UomDb : IUomDb
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public UomDb(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;

        }

        public IQueryable<InvUom> GetUomList()
        {
            return _context.InvUoms;
        }
    }
}
