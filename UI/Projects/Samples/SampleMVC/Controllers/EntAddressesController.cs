using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreLib.Models.SysDB;
using SampleMVC.Data;

namespace SampleMVC.Controllers
{
    public class EntAddressesController : Controller
    {
        private readonly SampleMVCContext _context;

        public EntAddressesController(SampleMVCContext context)
        {
            _context = context;
        }

        // GET: EntAddresses
        public async Task<IActionResult> Index()
        {
            var sampleMVCContext = _context.EntAddress.Include(e => e.EntCompany).Include(e => e.SysSetupType);
            return View(await sampleMVCContext.ToListAsync());
        }

        // GET: EntAddresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EntAddress == null)
            {
                return NotFound();
            }

            var entAddress = await _context.EntAddress
                .Include(e => e.EntCompany)
                .Include(e => e.SysSetupType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entAddress == null)
            {
                return NotFound();
            }

            return View(entAddress);
        }

        // GET: EntAddresses/Create
        public IActionResult Create()
        {
            ViewData["EntCompanyId"] = new SelectList(_context.Set<EntBusiness>(), "Id", "Id");
            ViewData["SysSetupTypeId"] = new SelectList(_context.Set<SysSetupType>(), "Id", "Id");
            return View();
        }

        // POST: EntAddresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EntCompanyId,SysSetupTypeId,add1,Add2,Add3,Add4,City,Remarks,Telno1,Telno2")] EntAddress entAddress)
        {
            if (ModelState.IsValid)
            {
                _context.Add(entAddress);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EntCompanyId"] = new SelectList(_context.Set<EntBusiness>(), "Id", "Id", entAddress.EntCompanyId);
            ViewData["SysSetupTypeId"] = new SelectList(_context.Set<SysSetupType>(), "Id", "Id", entAddress.SysSetupTypeId);
            return View(entAddress);
        }

        // GET: EntAddresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EntAddress == null)
            {
                return NotFound();
            }

            var entAddress = await _context.EntAddress.FindAsync(id);
            if (entAddress == null)
            {
                return NotFound();
            }
            ViewData["EntCompanyId"] = new SelectList(_context.Set<EntBusiness>(), "Id", "Id", entAddress.EntCompanyId);
            ViewData["SysSetupTypeId"] = new SelectList(_context.Set<SysSetupType>(), "Id", "Id", entAddress.SysSetupTypeId);
            return View(entAddress);
        }

        // POST: EntAddresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EntCompanyId,SysSetupTypeId,add1,Add2,Add3,Add4,City,Remarks,Telno1,Telno2")] EntAddress entAddress)
        {
            if (id != entAddress.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entAddress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntAddressExists(entAddress.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EntCompanyId"] = new SelectList(_context.Set<EntBusiness>(), "Id", "Id", entAddress.EntCompanyId);
            ViewData["SysSetupTypeId"] = new SelectList(_context.Set<SysSetupType>(), "Id", "Id", entAddress.SysSetupTypeId);
            return View(entAddress);
        }

        // GET: EntAddresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EntAddress == null)
            {
                return NotFound();
            }

            var entAddress = await _context.EntAddress
                .Include(e => e.EntCompany)
                .Include(e => e.SysSetupType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entAddress == null)
            {
                return NotFound();
            }

            return View(entAddress);
        }

        // POST: EntAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EntAddress == null)
            {
                return Problem("Entity set 'SampleMVCContext.EntAddress'  is null.");
            }
            var entAddress = await _context.EntAddress.FindAsync(id);
            if (entAddress != null)
            {
                _context.EntAddress.Remove(entAddress);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EntAddressExists(int id)
        {
          return _context.EntAddress.Any(e => e.Id == id);
        }
    }
}
