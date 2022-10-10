using InvWeb.Data;
using InvWeb.Data.Interfaces;
using InvWeb.Data.Models;
using InvWeb.Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebDBSchema.Models;

namespace InvWeb.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly UomServices uomServices;
        private readonly ItemServices itemServices;

        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
            uomServices = new UomServices(context);
            itemServices = new ItemServices(context);
        }


        // GET: api/Items/GetItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvItem>>> GetItems()
        {
            return await _context.InvItems.ToListAsync();
        }

        // GET: api/Items/GetDefaultUom/5
        [Route("api/Items/GetDefaultUom/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<int>> GetDefaultUom(int id)
        {
            if (id == 0)
            {
                return 0;
            }
            var item = await _context.InvItems.FindAsync(id);

            return  item.InvUomId;
        }


        // POST: api/Items/PostAddItemClassification/5 
        [HttpPost]
        public async Task<ActionResult<string>> PostAddItemClassification(int id, int classId)
        {
            try
            {

                if (id == 0 || classId == 0)
                {
                    return "error";
                }
                var item = _context.InvItems.Find(id);

                if (item == null)
                {
                    return "item not found";
                }

                InvItemClass InvItemClass = new InvItemClass();
                InvItemClass.InvItemId = id;
                InvItemClass.InvClassificationId = classId;

                _context.InvItemClasses.Add(InvItemClass);

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
