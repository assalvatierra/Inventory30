using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using CoreLib.DTO.Common.TrxHeader;
using CoreLib.Inventory.Interfaces;
using Microsoft.Extensions.Logging;
using Modules.Inventory;
using System.Security.Claims;

namespace InvWeb.Pages.Stores.Adjustment
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;
        private readonly IItemTrxServices itemTrxServices;

        public EditModel(ApplicationDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
            itemTrxServices = new ItemTrxServices(_context, _logger);
        }

        [BindProperty]
        public TrxHeaderCreateEditModel AdjustmentCreateModel { get; set; }
        public InvTrxHdr InvTrxHdr;
        public int StoreId { get; set; }
        //private int STATUS_ADJUSTMENT = 3;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvTrxHdr = await itemTrxServices.GetInvTrxHdrsByIdAsync((int)id);

            if (InvTrxHdr == null)
            {
                return NotFound();
            }

            this.SetStoreId((int)InvTrxHdr.InvStoreId);

            AdjustmentCreateModel = itemTrxServices.GetTrxHeaderEditModel_OnEditOnGet(InvTrxHdr, StoreId, GetUser());

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                AdjustmentCreateModel = itemTrxServices.GetTrxHeaderEditModel_OnEditOnGet(AdjustmentCreateModel.InvTrxHdr, StoreId, GetUser());
                return Page();
            }

            itemTrxServices.EditInvTrxHdrs(AdjustmentCreateModel.InvTrxHdr);

            try
            {
                await itemTrxServices.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvTrxHdrExists(InvTrxHdr.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index", new { storeId = AdjustmentCreateModel.InvTrxHdr.InvStoreId, Status = "PENDING" });
        }

        private bool InvTrxHdrExists(int id)
        {
            return _context.InvTrxHdrs.Any(e => e.Id == id);
        }
        private string GetUser()
        {
            return User.FindFirstValue(ClaimTypes.Name);
        }
        private void SetStoreId(int storeId)
        {
            StoreId = storeId;
        }
    }
}
