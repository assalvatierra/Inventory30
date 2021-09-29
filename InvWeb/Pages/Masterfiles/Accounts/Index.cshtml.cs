using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InvWeb.Data;
using WebDBSchema.Models.Users;

namespace InvWeb.Pages.Masterfiles.Accounts
{
    public class IndexModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public IndexModel(InvWeb.Data.ApplicationDbContext context)
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
                                      RoleNames = (from userRole in _context.UserRoles.Where(c=>c.UserId == user.Id)
                                                   join role in _context.Roles on userRole.RoleId
                                                   equals role.Id
                                                   select role.Name).ToList()
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
