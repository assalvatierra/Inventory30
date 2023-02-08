using CoreLib.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using Modules.Inventory;

namespace Inventory20
{
    public class SearchService20 : SearchServices
    {

       public SearchService20(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<InvTrxDtl>> GetApprovedInvDetailsAsync()
        {
            try
            {

                return await _context.InvTrxDtls
                     //.Where(i => i.InvTrxHdr.InvTrxHdrStatusId > STATUS_REQUEST)
                     .Include(i => i.InvItem)
                     .Include(i => i.InvTrxHdr)
                        .ThenInclude(i => i.InvStore)
                     .Include(i => i.InvUom)
                     .ToListAsync();
            }
            catch
            {
                throw new Exception("SearchServices: Unable to GetApprovedInvDetailsAsync");
            }

        }

    }
}