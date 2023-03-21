using CoreLib.DTO.PurchaseOrder;
using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Interfaces
{
    public interface IInvPOItemServices
    {

        public void CreateInvPoItem(InvPoItem invPoItem);
        public void EditInvPoItem(InvPoItem invPoItem);
        public Task DeleteInvPoItem(int id);
        public Task SaveChangesAsync();
        public bool InvPOItemExists(int id);

        public Task<InvPoItem> GetInvPoItemById(int id);

        public InvPOItemCreateEditModel GetInvPOItemModel_OnCreate(InvPOItemCreateEditModel InvPOItemCreate, int hdrId);
        public Task<InvPOItemCreateEditModel> GetInvPOItemModel_OnEdit(InvPOItemCreateEditModel InvPOItemEdit, int id);
        public Task<InvPOItemDelete> GetInvPOItemModel_OnDelete(InvPOItemDelete invPOItemDelete, int id);
    }
}
