using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;
using Microsoft.Extensions.Logging;
using InvWeb.Data.Services;
using NuGet.Versioning;

namespace InvWeb.Pages.Masterfiles.ItemMaster
{
    public class EditModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;
        private readonly ItemSpecServices _itemSpecServices;

        public EditModel(InvWeb.Data.ApplicationDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
            _itemSpecServices = new ItemSpecServices(context, logger);

        }

        [BindProperty]
        public InvItem InvItem { get; set; }

        [BindProperty]
        public InvItemSpec_Steel InvItemSpec_Steel { get; set; }

        [BindProperty]
        public Boolean showSpec { get; set; }

        public async Task<IActionResult> OnGetAsync( int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvItem = await _context.InvItems
                .Include(i => i.InvCategory)
                .Include(i => i.InvItemSpec_Steel)
                .Include(i => i.InvUom).FirstOrDefaultAsync(m => m.Id == id);

            if (InvItem == null)
            {
                return NotFound();
            }

            InvItemSpec_Steel = GetItemSpecDetails(InvItem.Id);
            this.showSpec = true;

            ViewData["InvCategoryId"] = new SelectList(_context.Set<InvCategory>(), "Id", "Description");
            ViewData["InvUomId"] = new SelectList(_context.Set<InvUom>(), "Id", "uom");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("InvItemSpec_Steel.Id");

            if (!ModelState.IsValid)
            {
                ViewData["InvCategoryId"] = new SelectList(_context.Set<InvCategory>(), "Id", "Description");
                ViewData["InvUomId"] = new SelectList(_context.Set<InvUom>(), "Id", "uom");
                return Page();
            }

            _context.Attach(InvItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvItemExists(InvItem.Id))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError("ItemsMasters Edit: Unable to update at OnPostAsync itemId:" + InvItem.Id);
                    throw;
                }
            }

            await UpdateItemSpecification(InvItemSpec_Steel, InvItem.Id);

            return RedirectToPage("./Index");
        }

        private bool InvItemExists(int id)
        {
            return _context.InvItems.Any(e => e.Id == id);
        }

        private InvItemSpec_Steel GetItemSpecDetails(int itemId)
        {
            try
            {
                var itemSpecLatest = _itemSpecServices.GetItemSpecificationByInvItemId(itemId);

                if (itemSpecLatest == null)
                {
                    return null;
                }

                return itemSpecLatest.FirstOrDefault();
            }
            catch
            {
                _logger.LogError("ItemsMasters Edit: Unable to GetItemSpecDetails itemId:" + itemId);
                return new InvItemSpec_Steel();
            }
        }

        private async Task<int> UpdateItemSpecification(InvItemSpec_Steel invItemSpec, int invItemId)
        {
            try
            {
                var isItemHaveSpecDetails = await _itemSpecServices.CheckItemHasAnyInvSpec(invItemId);
                if (isItemHaveSpecDetails)
                {
                    InvItemSpec_Steel.InvItemId = invItemId;

                    return await _itemSpecServices.EditItemSpecification(InvItemSpec_Steel);
                }

                InvItemSpec_Steel.InvItemId = invItemId;

                if (Validate_InvItemSpec_isValid())
                {
                    //if no item spec record, add item spec to item 
                    return await _itemSpecServices.AddItemSpecification(InvItemSpec_Steel);
                }

                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError("ItemsMasters Edit: Unable to UpdateItemSpecification itemId:" + invItemSpec.InvItemId + " " + ex.Message);
                return 0;
            }
        }

        private bool Validate_InvItemSpec_isValid()
        {
            if (InvItemSpec_Steel.InvItemId == 0)
            {
                return false;
            }

            if (String.IsNullOrEmpty(InvItemSpec_Steel.SpecFor))
            {
                return false;
            }

            return true;
        }
    }
}
