using CoreLib.Interfaces.DBAccess;
using CoreLib.Inventory.Models;
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
    public class InvPOHdrDb : IInvPOHdrDb
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public InvPOHdrDb(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }
        public void CreateInvPOHdr(InvPoHdr invPoHdr)
        {
            _context.InvPoHdrs.Add(invPoHdr);
        }

        public void DeleteInvPOHdr(InvPoHdr invPoHdr)
        {
            _context.InvPoHdrs.Remove(invPoHdr);
        }

        public void EditInvPOHdr(InvPoHdr invPoHdr)
        {
            _context.Attach(invPoHdr).State = EntityState.Modified;
        }

        public IQueryable<InvPoHdr> GetInvPOHdrs()
        {
            return _context.InvPoHdrs;
        }

        public async Task<InvPoHdr?> GetInvPOHdrAsync(int id)
        {
            return await _context.InvPoHdrs
               .Include(i => i.InvPoHdrStatu)
               .Include(i => i.InvStore)
               .Include(i => i.InvSupplier).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IQueryable<InvPoHdr>> GetInvPOHdrListAsync()
        {

            return (IQueryable<InvPoHdr>)await _context.InvPoHdrs
                .Include(i => i.InvPoHdrStatu)
                .Include(i => i.InvStore)
                .Include(i => i.InvSupplier)
                .Include(i => i.InvPoItems)
                    .ThenInclude(i => i.InvItem)
                    .ThenInclude(i => i.InvUom)
                .ToListAsync();
        }


        public async Task<InvPoHdr?> FindInvPOHdrByIdAsync(int id)
        {
            return await _context.InvPoHdrs.FindAsync(id);
        }
       
    }
}
