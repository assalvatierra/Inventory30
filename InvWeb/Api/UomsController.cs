using InvWeb.Data.Models;
using InvWeb.Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreLib.Inventory.Models;
using CoreLib.Inventory.Interfaces;
using CoreLib.Models.API;
using CoreLib.Models.Inventory;

namespace InvWeb.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UomsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUomServices uomServices;
        
        public UomsController(ApplicationDbContext context)
        {
            _context = context;
            uomServices = new UomServices(context);
        }


        // GET: api/Uoms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvStore>>> GetUoms()
        {
            return await _context.InvStores.ToListAsync();
        }

        // GET: api/Uoms/5
        [Route("api/Uoms/GetUom/{id}")]
        [HttpGet("{id}")]
        public IEnumerable<UomsApiModel.ItemOumList> GetUom(int id)
        {
            if (id == 0)
            {
                return new List<UomsApiModel.ItemOumList>();
            }
            
            return uomServices.GetItemUomListByItemId(id);
        }

    }
}
