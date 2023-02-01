using InvWeb.Data.Services;
using InvWeb.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CoreLib.Inventory.Models;
using InvWeb.Data.Models;
using System;
using Microsoft.Extensions.Logging;
using System.Linq;

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
            catch
            {
                //throw ex;
                return BadRequest();
            }

        }

        //PUD : /PutUpdateInvSteelSpec
        [HttpPost]
        public async Task<IActionResult> PutUpdateInvSteelSpec([FromBody] SpecsApiModel.Api_AddItem_SteelSpec item_Spec)
        {
            try
            {
                var InvSteelSpec = _itemSpecServices.GetItemSpecification_ByInvItemId(item_Spec.InvItemId);
                if (InvSteelSpec.FirstOrDefault() == null)
                {
                    // ADD ITEM STEEL SPEC 
                    InvItemSpec_Steel new_invItemSpec = new InvItemSpec_Steel();
                    new_invItemSpec.InvItemId = item_Spec.InvItemId;
                    new_invItemSpec.SpecFor = item_Spec.SpecFor;
                    new_invItemSpec.SpecInfo = item_Spec.SpecInfo;
                    new_invItemSpec.SizeValue = item_Spec.SizeValue;
                    new_invItemSpec.SizeDesc = item_Spec.SizeDesc;
                    new_invItemSpec.WtValue = item_Spec.WtValue;
                    new_invItemSpec.WtDesc = item_Spec.WtDesc;
                     
                    await _itemSpecServices.AddItemSpecification(new_invItemSpec);
                    return Ok();
                }

                InvItemSpec_Steel invItemSpec = InvSteelSpec.FirstOrDefault();

                invItemSpec.InvItemId = item_Spec.InvItemId;
                invItemSpec.SpecFor   = item_Spec.SpecFor;
                invItemSpec.SpecInfo  = item_Spec.SpecInfo;
                invItemSpec.SizeValue = item_Spec.SizeValue;
                invItemSpec.SizeDesc  = item_Spec.SizeDesc;
                invItemSpec.WtValue   = item_Spec.WtValue;
                invItemSpec.WtDesc    = item_Spec.WtDesc;

                await _itemSpecServices.EditItemSpecification(invItemSpec);

                return Ok();
            }
            catch
            {
                //throw ex;
                return BadRequest();
            }

        }
    }
}
