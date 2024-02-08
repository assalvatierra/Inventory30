using CoreLib.Interfaces;
using CoreLib.Inventory.Interfaces;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using Inventory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Modules.Inventory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public ObjectResult AddTrxDtlItem(int hdrId,  int invId, int qty, int uomId)
        {
            try
            {

                InvTrxDtl invTrxDtl = new InvTrxDtl();
                invTrxDtl.InvTrxHdrId = hdrId;
                invTrxDtl.InvItemId = invId;
                invTrxDtl.InvUomId = uomId;
                invTrxDtl.ItemQty = qty;
                invTrxDtl.InvTrxDtlOperatorId = 1;

                _context.InvTrxDtls.Add(invTrxDtl);
                _context.SaveChanges();

                return StatusCode(201, "Add Successfull");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Add not Successfull");
            }
        }


        [ActionName("DeleteTrxDtlItem")]
        [HttpDelete]
        public ObjectResult DeleteTrxDtlItem(int id)
        {
            try
            {

                InvTrxDtl invTrxDtl = itemDtlsServices.GetInvDtlsById(id).First();

                if (invTrxDtl == null)
                {
                    return StatusCode(500, "Unable to find item details");
                }

                _context.InvTrxDtls.Remove(invTrxDtl);
                _context.SaveChanges();

                return StatusCode(201, "Remove Successfull");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Add not Successfull");
            }
        }


        [ActionName("GetItemDetails")]
        [HttpGet]
        public string GetItemDetails(int id)
        {
            try
            {

                InvTrxDtl invTrxDtl = itemDtlsServices.GetInvDtlsById(id).First();

                if (invTrxDtl == null)
                {
                    return "Unable to find item details";
                }



                return JsonConvert.SerializeObject(new {
                     invTrxDtl.Id,
                     invTrxDtl.InvItemId,
                     invTrxDtl.InvUomId,
                     invTrxDtl.ItemQty
                });

            }
            catch (Exception ex)
            {
                return "Add not Successfull";
            }
        }
    }
}
