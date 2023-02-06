using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace InvWeb.Pages.Masterfiles.Accounts
{

    [Authorize(Roles = "ADMIN")]
    public class MainModel : PageModel
    {
        private readonly IdentityDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MainModel(IdentityDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<AppUser> AppUsers { get; set; }

        public void OnGet()
        {
          
            var usersFromDb = _context.Users.ToList();

            AppUsers = new List<AppUser>();
            foreach (var user in usersFromDb)
            {
                AppUser appUser = new()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Username = user.UserName
                };

                //get user roles
                var userroles = _context.UserRoles.Where(u => u.UserId == user.Id).ToList();

                userroles.ForEach(ur =>
                {
                    appUser.Role += _context.Roles.Where(r => r.Id == ur.RoleId).FirstOrDefault().Name + ", ";
                });

                AppUsers.Add(appUser);

            }
        }
    }
}
