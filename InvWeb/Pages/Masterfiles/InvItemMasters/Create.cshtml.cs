﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace InvWeb.Pages.Masterfiles.InvItemMasters
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["InvItemId"] = new SelectList(_context.Set<InvItem>(), "Id", "Description");
        ViewData["InvItemBrandId"] = new SelectList(_context.Set<InvItemBrand>(), "Id", "Name");
        ViewData["InvItemOriginId"] = new SelectList(_context.Set<InvItemOrigin>(), "Id", "Name");
        ViewData["InvUomId"] = new SelectList(_context.Set<InvUom>(), "Id", "uom");
            return Page();
        }

        [BindProperty]
        public InvItemMaster InvItemMaster { get; set; } = default!;
        

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
    }
}
