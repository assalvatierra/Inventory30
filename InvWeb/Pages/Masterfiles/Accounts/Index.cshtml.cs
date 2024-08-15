using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InvWeb.Data;
using CoreLib.Inventory.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace InvWeb.Pages.Masterfiles.Accounts
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly SecurityDbContext _context;

        public IndexModel(SecurityDbContext context)
        {
            _context = context;
        }

        public IList<AppUser> AppUsers { get; set; }

        public async Task OnGetAsync()
        {
          
             await _context.Users.ToListAsync();

        }

        //public void OnGet()
        //{

        //    var usersFromDb = _context.Users.ToList();
        //    AppUsers = null;
            //foreach (var user in usersFromDb)
            //{
            //    AppUser appUser = new AppUser();

            //    appUser.Id = user.Id;
            //    appUser.Email = user.Email;
            //    appUser.Username = user.UserName;

            //    //get user roles
            //    var userroles = _context.UserRoles.Where(u=>u.UserId == user.Id).ToList();

            //    userroles.ForEach(ur => {
            //        appUser.Role += _context.Roles.Where(r => r.Id == ur.RoleId) + ",";
            //    });

            //    AppUsers.Add(appUser);

            //}

            //var usersWithRoles = (from user in _context.Users
            //                      select new
            //                      {
            //                          UserId = user.Id,
            //                          Username = user.UserName,
            //                          user.Email,
            //                          RoleNames = (from userRole in _context.UserRoles.Where(c=>c.UserId == user.Id)
            //                                       join role in _context.Roles on userRole.RoleId
            //                                       equals role.Id
            //                                       select role.Name).ToList()
            //                      }).ToList().Select(p => new AppUser()
            //                      {
            //                          Id = p.UserId,
            //                          Username = p.Username,
            //                          Email = p.Email,
            //                          Role = string.Join(",", p.RoleNames)
            //                      });

            //AppUsers = usersWithRoles.ToList();
        //}
    }
}
