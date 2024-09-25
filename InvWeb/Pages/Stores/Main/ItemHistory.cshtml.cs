using System.Collections.Generic;
using System.Linq;
using CoreLib.Inventory.Interfaces;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Modules.Inventory;

namespace InvWeb.Pages.Stores.Main
{
    public class ItemHistoryModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;
        private readonly IStoreServices _storeSvc;

        public ItemHistoryModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            _storeSvc = new StoreServices(_context, logger);
        }

        public List<InvTrxDtl> Transactions { get; set; }

        public IActionResult OnGet(int id, int storeId)
        {

            if (id == 0 || storeId == 0)
            {
                return NotFound();
            }

            Transactions = _context.InvTrxDtls
                .Where(i => i.InvItemId == id && i.InvTrxHdr.InvStoreId == storeId)
                .Include(c=>c.InvItem)
                .ThenInclude(c=>c.InvCategory)
                .Include(c => c.InvTrxHdr)
                .ThenInclude(c => c.InvTrxHdrStatu)
                .Include(c => c.InvTrxHdr)
                .ThenInclude(c => c.InvTrxType)
                .Include(i=>i.InvTrxDtlOperator)
                .Include(i=>i.InvUom)
                .ToList();

            ViewData["StoreId"] = storeId;

            return Page();
        }
    }
}
