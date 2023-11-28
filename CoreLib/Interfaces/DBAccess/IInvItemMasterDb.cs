using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Interfaces.DBAccess
{
    public interface IInvItemMasterDb
    {
        public IQueryable<InvItemMaster> GetInvItemMasters();
        public Task<InvItemMaster?> GetInvItemMaster(int id);
        public Task<InvItemMaster> GetInvItemMasterAsync(int id);
        public Task<IQueryable<InvItemMaster>> GetInvItemMasterList();
        public void CreateInvItemMaster(InvItemMaster InvItemMaster);
        public void EditInvItemMaster(InvItemMaster InvItemMaster);
        public void DeleteInvItemMaster(InvItemMaster InvItemMaster);

    }
}
