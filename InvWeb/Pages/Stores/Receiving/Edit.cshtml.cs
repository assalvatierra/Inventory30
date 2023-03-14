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
using CoreLib.Inventory.Interfaces;
using Microsoft.Extensions.Logging;
using CoreLib.DTO.Receiving;
using Inventory.DBAccess;
using Modules.Inventory;

namespace InvWeb.Pages.Stores.Receiving
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EditModel> _logger;
        private readonly IItemTrxServices itemTrxServices;

        public EditModel(ApplicationDbContext context, ILogger<EditModel> logger)
        {
            _context = context;
            _logger = logger;
            itemTrxServices = new ItemTrxServices(_context, _logger);
        }

        [BindProperty]
        public ReceivingCreateEditModel ReceivingEditModel { get; set; }
        public InvTrxHdr InvTrxHdr;
        public int StoreId { get; set; }
        //private int STATUS_RECEIVING = 1;

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

            this.UpdateStoreId((int)InvTrxHdr.InvStoreId);

            ReceivingEditModel = itemTrxServices.GetReceivingEditModel_OnEditOnGet(InvTrxHdr, StoreId, GetUser());

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ReceivingEditModel = itemTrxServices.GetReceivingEditModel_OnEditOnGet(ReceivingEditModel.InvTrxHdr, StoreId, GetUser());
                return Page();
            }

            //_context.Attach(InvTrxHdr).State = EntityState.Modified;

            itemTrxServices.EditInvTrxHdrs(ReceivingEditModel.InvTrxHdr);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvTrxHdrExists(ReceivingEditModel.InvTrxHdr.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return RedirectToPage("./Index", new { storeId = InvTrxHdr.InvStoreId, status = "PENDING" });
            return RedirectToPage("./Details", new { id = ReceivingEditModel.InvTrxHdr.Id });
        }

        private bool InvTrxHdrExists(int id)
        {
            return itemTrxServices.InvTrxHdrExists(id); 
        }

        private string GetUser()
        {
            return User.FindFirstValue(ClaimTypes.Name);
        }

        private void UpdateStoreId(int storeId)
        {
            StoreId = storeId;
        }
    }
}
