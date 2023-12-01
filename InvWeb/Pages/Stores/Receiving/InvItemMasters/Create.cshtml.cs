using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLib.DTO.Common.Dialog;
using CoreLib.Inventory.Interfaces;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Modules.Inventory;

namespace InvWeb.Pages.Stores.Receiving.InvItemMasters
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IItemServices _itemServices;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
            _itemServices = new ItemServices(context);
        }

        public IActionResult OnGet(int itemId, int id)
        {
            var itemSpecs = _context.InvItemSpec_Steel.Where(i => i.InvItemId == itemId).FirstOrDefault();
            var item = _context.InvItems.Find(itemId);

            InvTrxDtlId = id;
            var DialogItems = ConvertItemsToDialogItems((List<InvItem>)_itemServices.GetInvItemsWithSteelSpecs());

            ViewData["SelectedInvItemId"] = itemId;
            ViewData["InvTrxDtlId"] = id;
            ViewData["DialogItems"] = ConvertItemsToDialogItems((List<InvItem>)_itemServices.GetInvItemsWithSteelSpecs());


            if (itemSpecs == null)
            {
                ViewData["InvItemId"] = new SelectList(_context.Set<InvItem>(), "Id", "Description", itemId);
                ViewData["InvItemBrandId"] = new SelectList(_context.Set<InvItemBrand>(), "Id", "Name");
                ViewData["InvItemOriginId"] = new SelectList(_context.Set<InvItemOrigin>(), "Id", "Name");
                ViewData["InvUomId"] = new SelectList(_context.Set<InvUom>(), "Id", "uom", item.InvUomId);
                return Page();
            }

            ViewData["InvItemId"] = new SelectList(_context.Set<InvItem>(), "Id", "Description", itemId);
            ViewData["InvItemBrandId"] = new SelectList(_context.Set<InvItemBrand>(), "Id", "Name", itemSpecs.SteelBrandId);
            ViewData["InvItemOriginId"] = new SelectList(_context.Set<InvItemOrigin>(), "Id", "Name", itemSpecs.SteelOriginId);
            ViewData["InvUomId"] = new SelectList(_context.Set<InvUom>(), "Id", "uom", item.InvUomId);
            return Page();
        }

        [BindProperty]
        public InvItemMaster InvItemMaster { get; set; } = default!;

        [BindProperty]
        public int InvTrxDtlId { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.InvItemMasters == null || InvItemMaster == null)
            {
                return Page();
            }

            _context.InvItemMasters.Add(InvItemMaster);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }



        public IEnumerable<DialogItems> ConvertItemsToDialogItems(List<InvItem> invItems)
        {
            List<DialogItems> dialogItems = new List<DialogItems>();

            foreach (InvItem item in invItems)
            {
                var itemspecs = "";
                if (item.InvItemSpec_Steel.Count > 0)
                {
                    var spec = item.InvItemSpec_Steel.First();

                    itemspecs = spec.SteelMainCat.Name + " " + spec.SteelMaterial.Name + " - " + spec.SteelBrand.Name + " " + spec.SteelOrigin.Name;
                }

                string remarkString = "";
                if (!String.IsNullOrEmpty(item.Remarks))
                {
                    remarkString = " - " + item.Remarks;
                }


                dialogItems.Add(new DialogItems
                {
                    Id = item.Id,
                    Name = item.InvCategory.Description + " - " + item.Description,
                    Description = itemspecs + remarkString
                });
            }

            return dialogItems;
        }
    }
}
