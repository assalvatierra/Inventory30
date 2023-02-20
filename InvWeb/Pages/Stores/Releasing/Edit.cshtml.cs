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
        public InvTrxHdr InvTrxHdr { get; set; }

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
            ViewData["InvStoreId"] = new SelectList(storeServices.GetInvStores(), "Id", "StoreName");
            ViewData["InvTrxHdrStatusId"] = new SelectList(itemTrxServices.GetInvTrxHdrStatus(), "Id", "Status");
            ViewData["InvTrxTypeId"] = new SelectList(itemTrxServices.GetInvTrxHdrTypes(), "Id", "Type", 2);
            ViewData["UserId"] = User.FindFirstValue(ClaimTypes.Name);
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

            //_context.Attach(InvTrxHdr).State = EntityState.Modified;
            itemTrxServices.EditInvTrxHdrs(InvTrxHdr);

            try
            {
                //await _context.SaveChangesAsync();

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

            return RedirectToPage("./Details", new { id = InvTrxHdr.Id });
        }

        private bool InvTrxHdrExists(int id)
        {
            return _context.InvTrxHdrs.Any(e => e.Id == id);
        }
    }
}
