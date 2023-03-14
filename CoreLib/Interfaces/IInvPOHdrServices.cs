using CoreLib.DTO.PurchaseOrder;
using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Interfaces
{
    public interface IInvPOHdrServices
    {
        public void CreateInvPoHdrs(InvPoHdr InvPoHdr);
        public void EditInvPoHdrs(InvPoHdr InvPoHdr);
        public void DeleteInvPoHdrs(InvPoHdr InvPoHdr);
        public Task SaveChangesAsync();
        public bool InvTrxDtlsExists(int id);

        public Task<InvPoHdr> GetInvPoHdrsbyIdAsync(int id);
        public Task<IEnumerable<InvPoHdr>> GetInvPoHdrsListAsync(int storeId);

        public Task<InvPOHdrModel> GetInvPOHdrModel_OnIndex(IList<InvPoHdr> InvPoHdrs, int storeId, string status, bool IsUserAdmin);
        public InvPOHdrCreateEditModel GetInvPOHdrModel_OnCreate(InvPOHdrCreateEditModel InvPoHdr, int storeId, string User);
        public InvPOHdrCreateEditModel GetInvPOHdrModel_OnEdit(InvPOHdrCreateEditModel InvPoHdr);
        public void RemoveInvPOHdrDeleteModel(InvPOHdrDeleteModel InvPoHdrDelete);
        public Task<InvPoHdr> InvPOHdrDelete_FindByIdAsync(int id);
    }
}
