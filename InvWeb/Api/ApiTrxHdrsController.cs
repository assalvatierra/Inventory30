using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLib.Inventory.Models;
using CoreLib.Inventory.Models.Items;
using CoreLib.Models.Inventory;
using CoreLib.Interfaces;
using Inventory;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using ReportViewModel.InvStore;

namespace InvWeb.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiTrxHdrsController : Controller
    {

        private readonly ApplicationDbContext _context;

        private IInvApprovalServices invApprovalServices;

        private readonly DateServices dateServices;

        public ApiTrxHdrsController(ApplicationDbContext context, ILogger<Controller> logger)
        {
            _context = context;
            invApprovalServices = new InvApprovalServices(context, logger);
            dateServices = new DateServices();
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
        [ActionName("PostHdrsCancelAsync")]
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
                    return StatusCode(400, "Not Found");
                    //return NotFound();
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


        // PUT: api/ApiTrxHdrs/PutHdrApprove/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostPOApproveAsync(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var trxHdr = await _context.InvPoHdrs.FindAsync(id);

            //APPROVED = 2
            trxHdr.InvPoHdrStatusId = 2;

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
        public async Task<IActionResult> PostPOCancelAsync(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var trxHdr = await _context.InvPoHdrs.FindAsync(id);

            //APPROVED = 2
            trxHdr.InvPoHdrStatusId = 4;

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


        [HttpGet]
        public ActionResult<IEnumerable<ItemLotNoSelect>> GetItemsLotNoList(int id, int storeId)
        {
           

            //Get Items at received with the same itemId
            var LotNoItems = _context.InvTrxDtls
                .Include(c=>c.InvItem)
                .Where(c => c.InvTrxHdr.InvTrxTypeId == 1
                    && c.InvTrxHdr.InvStoreId == storeId
                    && c.InvItemId == id).ToList();

            List<ItemLotNoSelect> lotNoSelects = new List<ItemLotNoSelect>();
            if (LotNoItems != null)
            {

                foreach (var i in LotNoItems)
                {
                    var newItem = new ItemLotNoSelect();
                    
                    newItem.Id = i.Id;
                    newItem.LotNo = i.InvTrxHdrId;
                    newItem.Description = "(" + i.InvItem.Code + ") " + i.InvItem.Description + " " + i.InvItem.Remarks;
                    newItem.Qty = 1;

                    lotNoSelects.Add(newItem);
                }

            }
            return lotNoSelects;
        }



        // PUT: api/ApiTrxHdrs/UpdateTrxHdrApproved/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> UpdateTrxHdrApproved(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var checkExists = invApprovalServices.InvTrxCheckHaveApprovalExist((int)id);
            // Create or Update Approval Trx
            if (checkExists)
            {
                var existingApproval = invApprovalServices.GetExistingApproval((int)id);

                if (existingApproval != null)
                {
                    existingApproval.ApprovedBy = GetUser();
                    existingApproval.ApprovedDate = dateServices.GetCurrentDateTime();

                    invApprovalServices.EditTrxApproval(existingApproval);
                    await invApprovalServices.SaveChangesAsync();
                }
            }
            else
            {
                //Create
                InvTrxApproval newApproval = new InvTrxApproval();
                newApproval.EncodedBy = GetUser();
                newApproval.EncodedDate = dateServices.GetCurrentDateTime();
                newApproval.ApprovedBy = GetUser();
                newApproval.ApprovedDate = dateServices.GetCurrentDateTime();
                newApproval.InvTrxHdrId = (int)id;

                invApprovalServices.CreateTrxApproval(newApproval);
                await invApprovalServices.SaveChangesAsync();

            }

            //Update Transaction Status
            if (invApprovalServices.CheckForApprovalStatus((int)id))
            {
                var trxHdr = await _context.InvTrxHdrs.FindAsync(id);

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
            }

            return StatusCode(200, "Status Update Successfull");
        }


        // PUT: api/ApiTrxHdrs/UpdateTrxHdrVerified/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> UpdateTrxHdrVerified(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            // Create or Update Approval Trx
            if (invApprovalServices.InvTrxCheckHaveApprovalExist((int)id))
            {
                var existingApproval = invApprovalServices.GetExistingApproval((int)id);

                if (existingApproval != null)
                {
                    existingApproval.VerifiedBy = GetUser();
                    existingApproval.VerifiedDate = dateServices.GetCurrentDateTime();

                    invApprovalServices.EditTrxApproval(existingApproval);
                    await invApprovalServices.SaveChangesAsync();
                }
            }
            else
            {
                //Create
                InvTrxApproval newApproval = new InvTrxApproval();
                newApproval.EncodedBy = GetUser();
                newApproval.EncodedDate = dateServices.GetCurrentDateTime();
                newApproval.VerifiedBy = GetUser();
                newApproval.VerifiedDate = dateServices.GetCurrentDateTime();
                newApproval.InvTrxHdrId = (int)id;

                invApprovalServices.CreateTrxApproval(newApproval);
                await invApprovalServices.SaveChangesAsync();

            }

            //Update Transaction Status
            if (invApprovalServices.CheckForApprovalStatus((int)id))
            {
                var trxHdr = await _context.InvTrxHdrs.FindAsync(id);

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
            }

            return StatusCode(200, "Status Update Successfull");
        }

        private string GetUser()
        {
            return User.FindFirstValue(ClaimTypes.Name);
        }
    }

}
