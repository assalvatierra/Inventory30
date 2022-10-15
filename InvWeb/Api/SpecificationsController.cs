using InvWeb.Data.Services;
using InvWeb.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebDBSchema.Models;
using InvWeb.Data.Models;

namespace InvWeb.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SpecificationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        //private readonly ItemSpecServices _itemSpecServices;

        public SpecificationsController(ApplicationDbContext context )
        {
            _context = context;
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
                InvItemCustomSpec invItemCustom = new InvItemCustomSpec();

                invItemCustom.InvCustomSpecId = item_CustomSpec.Id;
                invItemCustom.SpecValue = item_CustomSpec.SpecValue;
                invItemCustom.InvItemId = item_CustomSpec.InvItemId;


                _context.InvItemCustomSpecs.Add(invItemCustom);

                await _context.SaveChangesAsync();

                return Ok("OK");
            }
            catch
            {
                return NotFound("");
            }

        }
    }
}
