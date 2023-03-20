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
    public class InvPOItemDb : IInvPOItemDb
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public InvPOItemDb(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public void CreateInvPoItem(InvPoItem invPoItem)
        {
            _context.InvPoItems.Add(invPoItem);
        }

        public void DeleteInvPoItem(InvPoItem invPoItem)
        {
            _context.InvPoItems.Remove(invPoItem);
        }

        public void EditInvPoItem(InvPoItem invPoItem)
        {
            _context.Attach(invPoItem).State = EntityState.Modified;
        }

        public async Task<InvPoItem?> GetInvPoItem(int id)
        {
          
            return await _context.InvPoItems
                    .Include(i => i.InvItem)
                    .Include(i => i.InvPoHdr)
                    .Include(i => i.InvUom)
                    .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IQueryable<InvPoItem>> GetInvPoItemList()
        {
            return (IQueryable<InvPoItem>)await _context.InvPoItems
                .ToListAsync();
        }
    }
}
