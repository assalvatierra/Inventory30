﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.Stores.Releasing
{
    public class DetailsModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public DetailsModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public InvTrxHdr InvTrxHdr { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvTrxHdr = await _context.InvTrxHdrs
                .Include(i => i.InvStore)
                .Include(i => i.InvTrxHdrStatu)
                .Include(i => i.InvTrxDtls)
                .Include(i => i.InvTrxType).FirstOrDefaultAsync(m => m.Id == id);

            if (InvTrxHdr == null)
            {
                return NotFound();
            }

            ViewData["InvTrxDtls"] = await _context.InvTrxDtls
                .Where(i => i.InvTrxHdrId == id)
                .Include(i => i.InvUom)
                .Include(i => i.InvItem)
                .ToListAsync();

            return Page();
        }
    }
}