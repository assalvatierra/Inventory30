using CoreLib.Interfaces.DBAccess;
using CoreLib.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DBAccess
{
    public class InvTrxApprovalDb : IInvTrxApprovalDb
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public InvTrxApprovalDb(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Create(InvTrxApproval invTrxApproval)
        {
            _context.InvTrxApprovals.Add(invTrxApproval);
        }

        public void Delete(InvTrxApproval invTrxApproval)
        {
            _context.InvTrxApprovals.Remove(invTrxApproval);
        }

        public void Edit(InvTrxApproval invTrxApproval)
        {
            _context.Attach(invTrxApproval).State = EntityState.Modified;
        }

        public async Task<InvTrxApproval?> FindByIdAsync(int id)
        {
            return await _context.InvTrxApprovals.FindAsync(id);
        }

        public IQueryable<InvTrxApproval> Get()
        {
            return _context.InvTrxApprovals;
        }

        public async Task<InvTrxApproval?> GetAsync(int id)
        {
            return await _context.InvTrxApprovals.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IQueryable<InvTrxApproval>> GetListAsync()
        {
            return (IQueryable<InvTrxApproval>)await _context.InvTrxApprovals
                .ToListAsync();
        }
        public bool CheckExists(int id)
        {
            return _context.InvTrxApprovals.Any(e => e.Id == id);
        }
    }
}
