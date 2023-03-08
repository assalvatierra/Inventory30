using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Interfaces.DBAccess
{
    public interface IStoreDb
    {
        public InvStore? GetStore(int id);
        public IQueryable<InvStore> GetStoreList();
        public void CreateStoreList();
        public void EditStoreList();
        public void DeleteStoreList();
    }
}
