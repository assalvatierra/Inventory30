using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Interfaces.DBAccess
{
    public interface IInvTrxHdrStatusDb
    {
        public IQueryable<InvTrxHdrStatus> GetInvTrxHdrStatus();
    }
}
