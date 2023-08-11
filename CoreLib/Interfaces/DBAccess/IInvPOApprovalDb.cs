using CoreLib.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Interfaces.DBAccess
{
    public interface IInvPOApprovalDb
    {
        public Task<InvPOApproval?> GetAsync(int id);
        public Task<InvPOApproval?> FindByIdAsync(int id);
        public IQueryable<InvPOApproval> Get();
        public Task<IQueryable<InvPOApproval>> GetListAsync();
        public void Create(InvPOApproval invPoApproval);
        public void Edit(InvPOApproval invPoApproval);
        public void Delete(InvPOApproval invPoApproval);
        public bool CheckExists(int id);
    }
}
