﻿using InvWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDBSchema.Models;

namespace InvWeb.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiTrxHdrsController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ApiTrxHdrsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ApiTrxHdrs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvStore>>> GetTrxHdrs()
        {
            return await _context.InvStores.ToListAsync();
        }


        // PUT: api/ApiTrxHdrs/PutHdrApprove/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostHdrsApproveAsync(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var trxHdr = await _context.InvTrxHdrs.FindAsync(id);

            //APPROVED = 2
            trxHdr.InvTrxHdrStatusId = 2;

            _context.Entry(trxHdr).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvHdrExists((int)id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(400, "Exception error");
                    throw;
                }
            }

            return StatusCode(200, "Status Update Successfull");
        }

        // PUT: api/ApiTrxHdrs/PutHdrCancel/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostHdrsCancelAsync(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var trxHdr = await _context.InvTrxHdrs.FindAsync(id);

            //CANCELLED = 4
            trxHdr.InvTrxHdrStatusId = 4;

            _context.Entry(trxHdr).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvHdrExists((int)id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(400, "Exception error");
                    throw;
                }
            }

            return StatusCode(200, "Status Update Successfull");
        }

        private bool InvHdrExists(int id)
        {
            return _context.InvPoHdrs.Any(e => e.Id == id);
        }
    }
}