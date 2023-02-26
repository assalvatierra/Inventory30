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
using Modules.Inventory;
using CoreLib.DTO.Releasing;

namespace InvWeb.Pages.Stores.Releasing
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EditModel> _logger;
        private readonly IItemTrxServices itemTrxServices;
        private readonly IStoreServices storeServices;

        public EditModel(ILogger<EditModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            itemTrxServices = new ItemTrxServices(_context, _logger);
            storeServices = new StoreServices(_context, _logger);
        }

        [BindProperty]
        public ReleasingCreateEditModel ReleasingEditModel { get; set; }
        public InvTrxHdr InvTrxHdr;
        public int StoreId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvTrxHdr = await itemTrxServices.GetInvTrxHdrsById((int)id)
                                             .FirstOrDefaultAsync();

            if (InvTrxHdr == null)
            {
                return NotFound();
            }

            UpdateStoreId(InvTrxHdr.InvStoreId);

            ReleasingEditModel = itemTrxServices.GetReleasingEditModel_OnEditOnGet(InvTrxHdr, StoreId, GetUser(), GetStores());

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ReleasingEditModel = itemTrxServices.GetReleasingEditModel_OnEditOnGet(ReleasingEditModel.InvTrxHdr, StoreId, GetUser(), GetStores());
                return Page();
            }

            itemTrxServices.EditInvTrxHdrs(ReleasingEditModel.InvTrxHdr);

            try
            {
                await itemTrxServices.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvTrxHdrExists(ReleasingEditModel.InvTrxHdr.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Details", new { id = ReleasingEditModel.InvTrxHdr.Id });
        }

        private bool InvTrxHdrExists(int id)
        {
            return _context.InvTrxHdrs.Any(e => e.Id == id);
        }

        private string GetUser()
        {
            return User.FindFirstValue(ClaimTypes.Name);
        }

        private List<InvStore> GetStores()
        {
            return storeServices.GetInvStores().ToList();
        }

        private void UpdateStoreId(int storeId)
        {
            StoreId = storeId;
        }
    }
}
