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
    public class InvSupplierDb : IInvSupplierDb
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public InvSupplierDb(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public InvSupplier GetInvSupplierbyId(int id)
        {
            return _context.InvSuppliers.Find(id);
        }

        public IList<InvSupplier> GetInvSuppliers()
        {
            return _context.InvSuppliers.ToList();
        }
    }
}
