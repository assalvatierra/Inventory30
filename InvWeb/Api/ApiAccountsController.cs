using InvWeb.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDBSchema.Models.Users;
using Newtonsoft.Json;

namespace InvWeb.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiAccountsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ApiAccountsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/<ApiAccountController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ApiAccountController>/5
        [HttpGet("{id}")]
        public async Task<string> GetAsync(int id)
        {
            var store = await _context.InvStores.FindAsync(id);

            return store.StoreName;
        }

        // POST api/ApiAccounts/PostAddUserRole
        [HttpPost]
        public async Task<IActionResult> PostAddUserRoleAsync(string userId, int roleId)
        {
            try
            {
                //var user = await _userManager.FindByIdAsync(userId);
                //var role = await _context.Roles.FindAsync(roleId.ToString());
                ////_ = await _userManager.AddToRoleAsync(user, role.Name.ToUpper());

                _context.UserRoles.Add(new IdentityUserRole<string>
                {
                    RoleId = roleId.ToString(),
                    UserId = userId
                });

                await _context.SaveChangesAsync();
                return StatusCode(200, "Add Successfull");
            }
            catch
            {
                return StatusCode(400, "error execeiption encounterd");
            }

        }


        // POST api/ApiAccounts/RemoveUserRole
        [HttpDelete]
        public async Task<IActionResult> RemoveUserRoleAsync(string userId, int roleId)
        {
            try
            {
                var userRole = _context.UserRoles.Where(r => r.RoleId == roleId.ToString() && r.UserId == userId).FirstOrDefault();

                if (userRole == null)
                {
                    return StatusCode(400, "Not found");
                }

                _context.UserRoles.Remove(userRole);

                await _context.SaveChangesAsync();

                return StatusCode(200, "Deletion Successfull");
            }
            catch
            {
                return StatusCode(400, "Error encounter");
            }
        }

        //// PUT api/<ApiAccountController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<ApiAccountController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
