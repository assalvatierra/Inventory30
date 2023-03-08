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
    public class StoreDb : IStoreDb
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public StoreDb(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;

        }

        public void CreateStoreList()
        {
            throw new NotImplementedException();
        }

        public void DeleteStoreList()
        {
            throw new NotImplementedException();
        }

        public void EditStoreList()
        {
            throw new NotImplementedException();
        }

        public InvStore? GetStore(int id)
        {
            return _context.InvStores.Find(id);
        }


        public IQueryable<InvStore> GetStoreList()
        {
            return _context.InvStores;
        }
    }
}
