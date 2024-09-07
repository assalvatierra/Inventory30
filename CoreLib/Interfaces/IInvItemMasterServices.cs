using CoreLib.DTO.PurchaseOrder;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Interfaces
{
    public interface IInvItemMasterServices
    {

        public Task CreateInvItemMaster(InvItemMaster invItemMaster);
        public void EditInvItemMaster(InvItemMaster invItemMaster);
        public Task DeleteInvItemMaster(int id);
        public Task DeleteInvItemMasterLink(int id);
        public Task SaveChangesAsync();
        public bool InvItemMasterExists(int id);

        public Task<InvItemMaster> GetInvItemMasterById(int id);

        public Task CreateItemMasterInvDtlsLink(int invItemMasterId, int invDtlsId);

        public Task<List<InvItemMaster>> GetInvItemMasters_Received_ById(int invItemId);
        public List<InvTrxDtlxItemMaster> GetInvTrxDtlxItemMaster_byId(int InvItemMasterId);

    }
}
