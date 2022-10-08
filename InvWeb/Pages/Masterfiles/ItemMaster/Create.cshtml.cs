using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InvWeb.Data;
using WebDBSchema.Models;
using InvWeb.Data.Services;
using Microsoft.Extensions.Logging;

namespace InvWeb.Pages.Masterfiles.ItemMaster
{
    public class CreateModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;
        private readonly ItemSpecServices _itemSpecServices;

        public CreateModel(InvWeb.Data.ApplicationDbContext context, ILogger<CreateModel> logger)
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

            //update showSpec flag based on Category
            this.showSpec = _itemSpecServices.IsCategoryHaveSpecDefs(categoryId);

            ViewData["InvCategoryId"] = new SelectList(_context.Set<InvCategory>(), "Id", "Description", categoryId);
            ViewData["InvUomId"] = new SelectList(_context.Set<InvUom>(), "Id", "uom");
            return Page();
        }

        [BindProperty]
        public InvItem InvItem { get; set; }

        [BindProperty]
        public InvItemSpec_Steel InvItemSpec_Steel { get; set; }

        [BindProperty]
        public Boolean showSpec { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InvItems.Add(InvItem);
            await _context.SaveChangesAsync();

            await AddItemSpecToItem();

            return RedirectToPage("./Index");
        }

        public async Task<int> AddItemSpecToItem()
        {
            if (!ModelState.IsValid)
            {
                return 0;
            }

            if (InvItem.Id == 0 || InvItem == null)
            {
                return 0;
            }

            if (String.IsNullOrEmpty(InvItemSpec_Steel.SpecFor))
            {
                return 0;
            }

            InvItemSpec_Steel.InvItemId = InvItem.Id;

            return await _itemSpecServices.AddItemSpecification(InvItemSpec_Steel);
        }
    }
}
