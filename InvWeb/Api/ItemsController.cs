using InvWeb.Data;
using InvWeb.Data.Interfaces;
using InvWeb.Data.Models;
using InvWeb.Data.Services;
using Microsoft.AspNetCore.Http;
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


        // GET: api/Items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvItem>>> GetItems()
        {
            return await _context.InvItems.ToListAsync();
        }

        // GET: api/Items/5
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


    }
}
