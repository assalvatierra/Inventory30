using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Interfaces.DBAccess
{
    public interface IInvPOHdrDb
    {
        public Task<InvPoHdr?> GetInvPOHdrAsync(int id);
        public Task<IQueryable<InvPoHdr>> GetInvPOHdrListAsync();
        public void CreateInvPOHdr(InvPoHdr invPoHdr);
        public void EditInvPOHdr(InvPoHdr invPoHdr);
        public void DeleteInvPOHdr(InvPoHdr invPoHdr);
    }
}
