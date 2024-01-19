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
    public class InvItemMasterDb : IInvItemMasterDb
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public InvItemMasterDb(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public IQueryable<InvItemMaster> GetInvItemMasters()
        {
            return _context.InvItemMasters;
        }

        public void CreateInvItemMaster(InvItemMaster InvItemMaster)
        {
            _context.InvItemMasters.Add(InvItemMaster);
        }

        public void DeleteInvItemMaster(InvItemMaster InvItemMaster)
        {
            _context.InvItemMasters.Remove(InvItemMaster);
        }

        public void EditInvItemMaster(InvItemMaster InvItemMaster)
        {
            _context.Attach(InvItemMaster).State = EntityState.Modified;
        }

        public async Task<InvItemMaster?> GetInvItemMaster(int id)
        {
          
            return await _context.InvItemMasters
                    .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<InvItemMaster> GetInvItemMasterAsync(int id)
        {
            return await _context.InvItemMasters.FindAsync(id);
        }

        public async Task<IQueryable<InvItemMaster>> GetInvItemMasterList()
        {
            return (IQueryable<InvItemMaster>)await _context.InvItemMasters
                .ToListAsync();
        }
    }
}
