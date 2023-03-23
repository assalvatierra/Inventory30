using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using System.Security.Claims;
using CoreLib.Models.Inventory;
using Inventory;
using Microsoft.Extensions.Logging;
using CoreLib.Interfaces;
using CoreLib.DTO.PurchaseOrder;

namespace InvWeb.Pages.Stores.PurchaseRequest
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IInvPOHdrServices invPOHdrServices;
        private readonly ILogger<IndexModel> _logger;

        public EditModel(ApplicationDbContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
            invPOHdrServices = new InvPOHdrServices(_context, _logger);
        }

        [BindProperty]
        public InvPOHdrCreateEditModel POHdrEditModel { get; set; }
        public InvPoHdr InvPoHdr { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            POHdrEditModel = new InvPOHdrCreateEditModel();

            var invPoHdr =  await invPOHdrServices.GetInvPoHdrsbyIdAsync((int)id);

            if (invPoHdr == null)
            {
                return NotFound();
            }

            POHdrEditModel = invPOHdrServices.GetInvPOHdrModel_OnEdit(POHdrEditModel);
            POHdrEditModel.InvPoHdr = invPoHdr;

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            invPOHdrServices.EditInvPoHdrs(POHdrEditModel.InvPoHdr);

            try
            {
                await invPOHdrServices.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvPoHdrExists(POHdrEditModel.InvPoHdr.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index", new { storeId = POHdrEditModel.InvPoHdr.InvStoreId });
        }

        private bool InvPoHdrExists(int id)
        {
            return invPOHdrServices.InvTrxDtlsExists(id);
        }
    }
}
