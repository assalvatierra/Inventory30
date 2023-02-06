using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvWeb.Data;
using CoreLib.Inventory.Models.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace InvWeb.Data.Services
{
    public class UserServices
    {
        private readonly IdentityDbContext _context;

        public UserServices(IdentityDbContext context)
        {
            _context = context;
        }

        public List<AppUser> GetUserList()
        {
            try
            {

                var usersWithRoles = (from user in _context.Users
                                  select new
                                  {
                                      UserId = user.Id,
                                      Username = user.UserName,
                                      user.Email,
                                      RoleNames = "NA"
                                      //RoleNames = (from userRole in user.
                                      //             join role in _context.Roles on userRole.RoleId
                                      //             equals role.Id
                                      //             select role.Name).ToList()
                                  }).ToList().Select(p => new AppUser()

                                  {
                                      Id = p.UserId,
                                      Username = p.Username,
                                      Email = p.Email,
                                      Role = string.Join(",", p.RoleNames)
                                  });

                return usersWithRoles.ToList();
            }
            catch
            {

                throw new Exception("UserServices: Unable to GetUserList");
            }
        }
    }
}
