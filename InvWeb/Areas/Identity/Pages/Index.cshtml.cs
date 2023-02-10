using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using CoreLib.Inventory.Models.Users;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace InvWeb.Areas.Identity.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IdentityDbContext _context;

        public IndexModel(IdentityDbContext context)
        {
            _context = context;
        }

        public IList<AppUser> AppUsers { get; set; }

        public void OnGet()
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

            AppUsers = usersWithRoles.ToList();
        }
    }
}
