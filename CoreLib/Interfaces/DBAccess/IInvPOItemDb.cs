using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Interfaces.DBAccess
{
    public interface IInvPOItemDb
    {
        public Task<InvPoItem?> GetInvPoItem(int id);
        public Task<IQueryable<InvPoItem>> GetInvPoItemList();
        public void CreateInvPoItem(InvPoItem invPoItem);
        public void EditInvPoItem(InvPoItem invPoItem);
        public void DeleteInvPoItem(InvPoItem invPoItem);
    }
}
