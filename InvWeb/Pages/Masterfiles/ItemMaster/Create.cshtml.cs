using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreLib.Inventory.Models;
using InvWeb.Data.Services;
using Microsoft.Extensions.Logging;
using CoreLib.Models.Inventory;
using Modules.Inventory;
using CoreLib.Inventory.Interfaces;

namespace InvWeb.Pages.Masterfiles.ItemMaster
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;
        private readonly IItemSpecServices _itemSpecServices;

        public CreateModel(ApplicationDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
            _itemSpecServices = new ItemSpecServices(context, logger);
        }

        public IActionResult OnGet(string desc,string code, int? categoryId, string remarks, int? uomId)
        {

            //load InvItem on page reload 
            if (!String.IsNullOrEmpty(code))
            {
                InvItem = new InvItem();
                InvItem.Description = desc;
                InvItem.Code = code;
                InvItem.Remarks = remarks;

                if (categoryId  != null)
                {
                    InvItem.InvCategoryId = (int)categoryId;
                }

                if (uomId != null)
                {
                    InvItem.InvUomId = (int)uomId;
                }
            }

            //set initial category to default = 1
            if(categoryId == null)
            {
                categoryId = 1;
            }

            ViewData["InvCategoryId"] = new SelectList(_context.Set<InvCategory>(), "Id", "Description", categoryId);
            ViewData["InvUomId"] = new SelectList(_context.Set<InvUom>(), "Id", "uom");
            return Page();
        }

        [BindProperty]
        public InvItem InvItem { get; set; }

        [BindProperty]
        public InvItemSpec_Steel InvItemSpec_Steel { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InvItems.Add(InvItem);
            await _context.SaveChangesAsync();


            return RedirectToPage("./Index");
        }

    }
}
