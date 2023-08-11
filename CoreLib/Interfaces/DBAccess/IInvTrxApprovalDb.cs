using CoreLib.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Interfaces.DBAccess
{
    public interface IInvTrxApprovalDb
    {
        public Task<InvTrxApproval?> GetAsync(int id);
        public Task<InvTrxApproval?> FindByIdAsync(int id);
        public IQueryable<InvTrxApproval> Get();
        public Task<IQueryable<InvTrxApproval>> GetListAsync();
        public void Create(InvTrxApproval invTrxApproval);
        public void Edit(InvTrxApproval invTrxApproval);
        public void Delete(InvTrxApproval invTrxApproval);
        public bool CheckExists(int id);
    }
}
