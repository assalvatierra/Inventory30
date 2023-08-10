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
    public class InvPOApprovalDb : IInvPOApprovalDb
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public InvPOApprovalDb(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }
        public void Create(InvPOApproval invPoApproval)
        {
            _context.InvPoApprovals.Add(invPoApproval);
        }

        public void Delete(InvPOApproval invPoApproval)
        {
            _context.InvPoApprovals.Remove(invPoApproval);
        }

        public void Edit(InvPOApproval invPoApproval)
        {
            _context.Attach(invPoApproval).State = EntityState.Modified;
        }

        public async Task<InvPOApproval?> FindByIdAsync(int id)
        {
            return await _context.InvPoApprovals.FindAsync(id);
        }

        public IQueryable<InvPOApproval> Get()
        {
            return _context.InvPoApprovals;
        }

        public async Task<InvPOApproval?> GetAsync(int id)
        {
            return await _context.InvPoApprovals.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IQueryable<InvPOApproval>> GetListAsync()
        {
            return (IQueryable<InvPOApproval>)await _context.InvPoApprovals
                .ToListAsync();
        }
    }
}
