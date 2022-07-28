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
using PageObjectShared;
using PageObjectShared.Interfaces;

namespace InvWeb.Pages.xTestPages.Edit01
{
    public class EditModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;
        public IObjectConfigServices _objectConfigServices;
        public EditModel(InvWeb.Data.ApplicationDbContext context, IObjectConfigServices objectConfigServices)
        {
            _context = context;

            this._objectConfigServices = objectConfigServices;
        }

        [BindProperty]
        public InvCategory InvCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //this._objectConfigServices

            InvCategory = await _context.InvCategories.FirstOrDefaultAsync(m => m.Id == id);

            if (InvCategory == null)
            {
                return NotFound();
            }
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

            _context.Attach(InvCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvCategoryExists(InvCategory.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool InvCategoryExists(int id)
        {
            return _context.InvCategories.Any(e => e.Id == id);
        }
    }
}
