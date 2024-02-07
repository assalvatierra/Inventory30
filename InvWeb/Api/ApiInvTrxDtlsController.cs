using CoreLib.Interfaces;
using CoreLib.Inventory.Interfaces;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using Inventory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Modules.Inventory;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InvWeb.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiInvTrxDtlsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly IItemTrxServices itemTrxServices;
        private readonly IInvApprovalServices invApprovalServices;
        private readonly IItemDtlsServices itemDtlsServices;
        private readonly DateServices dateServices;

        public ApiInvTrxDtlsController(ApplicationDbContext context, ILogger<Controller> logger)
        {
            _context = context;
            dateServices = new DateServices();
            invApprovalServices = new InvApprovalServices(context, logger);
            itemDtlsServices = new ItemDtlsServices(context, logger);
            itemTrxServices = new ItemTrxServices(context, logger);
        }

        // GET: api/<ApiInvTrxDtlsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ApiInvTrxDtlsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ApiInvTrxDtlsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ApiInvTrxDtlsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ApiInvTrxDtlsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [ActionName("AddTrxDtlItem")]
        [HttpPost]
        public ObjectResult AddTrxDtlItem(int hdrId,  int invId, int qty, int uomId, string remarks)
        {
            try
            {

                InvTrxDtl invTrxDtl = new InvTrxDtl();
                invTrxDtl.InvTrxHdrId = hdrId;
                invTrxDtl.InvItemId = invId;
                invTrxDtl.InvUomId = uomId;
                invTrxDtl.ItemQty = qty;
                invTrxDtl.InvTrxDtlOperatorId = 1;

                //invTrxDtl.remarks = qty;

                itemDtlsServices.CreateInvDtls(invTrxDtl);
                itemDtlsServices.SaveChangesAsync();

                return StatusCode(201, "Add Successfull");
            }
            catch
            {
                return StatusCode(500, "Add not Successfull");
            }
        }
    }
}
