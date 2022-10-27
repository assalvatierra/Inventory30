using InvWeb.Data.Services;
using InvWeb.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebDBSchema.Models;
using InvWeb.Data.Models;
using System;
using Microsoft.Extensions.Logging;

namespace InvWeb.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SpecificationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ItemSpecServices _itemSpecServices;
        private readonly ILogger<SpecificationsController> _logger;

        public SpecificationsController(ApplicationDbContext context, ILogger<SpecificationsController> logger)
        {
            _context = context;
            _logger = logger;
            _itemSpecServices = new ItemSpecServices(context, logger);
        }


        // POST: api/Specifications/InvCategorySpecDefs
        [HttpPost]
        public async Task<ActionResult<string>> PostAddInvCategorySpecDefs(int specId, int catId)
        {
            try
            {

                if (catId == 0 || specId == 0)
                {
                    return "error";
                }
                var category = _context.InvCategories.Find(catId);

                if (category == null)
                {
                    return "category not found";
                }

                InvCategorySpecDef InvCategorySpecDefs = new InvCategorySpecDef();
                InvCategorySpecDefs.InvItemSysDefinedSpecsId = specId;
                InvCategorySpecDefs.InvCategoryId = catId;

                _context.InvCategorySpecDefs.Add(InvCategorySpecDefs);

                await _context.SaveChangesAsync();

                return "success";
            }
            catch
            {
                return "error on catch";
            }
        }

        // POST: api/Specifications/PostAddInvCustomSpec
        [HttpPost]
        public async Task<IActionResult> PostAddInvCustomSpec([FromBody] SpecsApiModel.Api_AddItem_CustomSpec item_CustomSpec)
        {
            try
            {
                var InvCustomSpec = _itemSpecServices.GetCustomSpecification(item_CustomSpec.CustomSpecId);
                if (InvCustomSpec == null)
                {
                    return NotFound();
                }

                InvItemCustomSpec invItemCustom = new InvItemCustomSpec();
                invItemCustom.InvCustomSpecId = item_CustomSpec.CustomSpecId;
                invItemCustom.SpecValue = item_CustomSpec.SpecValue;
                invItemCustom.InvItemId = item_CustomSpec.InvItemId;
                invItemCustom.Order = InvCustomSpec.Order.ToString();
                invItemCustom.Remarks = item_CustomSpec.Remarks;

                await _itemSpecServices.AddItemCustomSpecification(invItemCustom);

                return Ok();
            }
            catch 
            {
                return BadRequest();
            }

        }


        // POST: api/Specifications/PutUpdateInvCustomSpec
        [HttpPost]
        public async Task<IActionResult> PutUpdateInvCustomSpec([FromBody] SpecsApiModel.Api_AddItem_CustomSpec item_CustomSpec)
        {
            try
            {
                var InvCustomSpec = _itemSpecServices.GetItemCustomSpecification(item_CustomSpec.Id);
                if (InvCustomSpec == null)
                {
                    return NotFound();
                }

                InvCustomSpec.InvCustomSpecId = item_CustomSpec.CustomSpecId;
                InvCustomSpec.SpecValue = item_CustomSpec.SpecValue;
                InvCustomSpec.InvItemId = item_CustomSpec.InvItemId;
                InvCustomSpec.Remarks = item_CustomSpec.Remarks;

                await _itemSpecServices.EditItemCustomSpecification(InvCustomSpec);

                return Ok();
            }
            catch(Exception ex)
            {
                //throw ex;
                return BadRequest();
            }

        }
    }
}
