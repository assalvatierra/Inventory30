﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using Microsoft.AspNetCore.Authorization;
using CoreLib.Models.Inventory;

namespace InvWeb.Suppliers.Supplier
{
    [Authorize(Roles = "ADMIN")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvSupplier> InvSupplier { get;set; }

        public async Task OnGetAsync()
        {
            InvSupplier = await _context.InvSuppliers.ToListAsync();
        }
    }
}