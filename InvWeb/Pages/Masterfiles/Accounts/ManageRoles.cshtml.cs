using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebDBSchema.Models.Users;

namespace InvWeb.Pages.Masterfiles.Accounts
{
    [Authorize(Roles = "ADMIN")]
    public class ManageRolesModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public ManageRolesModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<AppUserRole> AppUserRoles { get; set; }

        public void OnGet(string id)
        {
            AppUserRoles = new List<AppUserRole>();
            //get user roles
            var userroles = _context.UserRoles.Where(u => u.UserId == id).ToList();

            userroles.ForEach(r => {


                AppUserRoles.Add(new AppUserRole
                {
                    Id = 0,
                    RoleId = r.RoleId,
                    UserId = r.UserId,
                    RoleName = _context.Roles.Find(r.RoleId).Name
                });
            });

            ViewData["User"] = id;
            ViewData["Roles"] = _context.Roles.ToList();
        }
    }
}
