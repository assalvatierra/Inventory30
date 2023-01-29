using InvWeb.Data.Services;
using InvWeb.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreLib.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InvWeb.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly ItemServices itemServices;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
            itemServices = new ItemServices(context);
        }


        // GET: api/Categories/GetCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvItem>>> GetCategories()
        {
            return await _context.InvItems.ToListAsync();
        }

        // GET: api/Categories/GetCategoryDefinedSpecsCount
        [Route("api/Categories/GetCategoryDefinedSpecsCount/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<int>> GetCategoryDefinedSpecsCount(int id)
        {
            if (id == 0)
            {
                return 0;
            }
            var itemCategory = await _context.InvCategories
                                    .Include(i=>i.InvCategorySpecDefs)
                                    .Where(i=>i.Id == id)
                                    .ToListAsync();
            if (itemCategory != null)
            {
                return itemCategory.FirstOrDefault().InvCategorySpecDefs.Count();
            }

            return 0;
        }


        // POST: api/Categories/InvCategorySpecDefs
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


    }
}
