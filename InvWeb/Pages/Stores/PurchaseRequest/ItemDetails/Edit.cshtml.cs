using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using InvWeb.Data.Services;
using CoreLib.Inventory.Interfaces;
using CoreLib.Models.Inventory;
using Modules.Inventory;
using CoreLib.Interfaces;
using Microsoft.Extensions.Logging;
using Inventory;
using CoreLib.DTO.PurchaseOrder;

namespace InvWeb.Pages.Stores.PurchaseRequest.ItemDetails
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EditModel> _logger;
        private readonly IInvPOItemServices invPOItemServices;

        public EditModel(ApplicationDbContext context, ILogger<EditModel> logger)
        {
            _context = context;
            _logger = logger;
            invPOItemServices = new InvPOItemServices(_context, _logger);
        }

        [BindProperty]
        public InvPOItemCreateEditModel InvPOItemEdit { get; set; }
        public InvPoItem InvPoItem { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvPOItemEdit = await invPOItemServices.GetInvPOItemModel_OnEdit(InvPOItemEdit, (int)id);

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

            //_context.Attach(InvPoItem).State = EntityState.Modified;
            invPOItemServices.EditInvPoItem(InvPOItemEdit.InvPoItem);

            try
            {
                //await _context.SaveChangesAsync();
                await invPOItemServices.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvPoItemExists(InvPoItem.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../Details", new { id = InvPOItemEdit.InvPoItem.InvPoHdrId });
        }

        private bool InvPoItemExists(int id)
        {
            return invPOItemServices.InvPOItemExists(id);
        }
    }
}
