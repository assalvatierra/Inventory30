using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Interfaces;
using Microsoft.Extensions.Logging;
using Modules.Inventory;
using CoreLib.DTO.Common.TrxHeader;

namespace InvWeb.Pages.Stores.Adjustment
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DeleteModel> _logger;
        private readonly IItemTrxServices itemTrxServices;

        public DetailsModel(ApplicationDbContext context, ILogger<DeleteModel> logger)
        {
            _context = context;
            _logger = logger;
            _context = context;
            itemTrxServices = new ItemTrxServices(_context, _logger);
        }

        [BindProperty]
        public TrxHeaderDetailsModel HeaderDetailsModel { get; set; }

        //public InvTrxHdr InvTrxHdr { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HeaderDetailsModel =  await itemTrxServices.GetTrxHeaderDetailsModel_OnDetailsOnGet((int)id);

            if (HeaderDetailsModel.InvTrxHdr == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
