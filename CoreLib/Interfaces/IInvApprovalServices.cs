using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Interfaces
{
    public interface IInvApprovalServices
    {
        public void CreateTrxApproval(InvTrxApproval invTrxApproval);
        public void CreatePoApproval(InvPOApproval invPOApproval);
        public void EditTrxApproval(InvTrxApproval invTrxApproval);
        public void EditPoApproval(InvPOApproval invPOApproval);
        public void DeleteTrxApproval(InvTrxApproval invTrxApproval);
        public void DeletePoApproval(InvPOApproval invPOApproval);
        public bool InvTrxApprovalExists(int id);
        public bool InvPoApprovalExists(int id);
        public bool InvTrxCheckHaveApprovalExist(int TrxId);
        public InvTrxApproval GetExistingApproval(int TrxId);
        public Task SaveChangesAsync();

        public bool CheckForApprovalStatus(int id);
    }
}
