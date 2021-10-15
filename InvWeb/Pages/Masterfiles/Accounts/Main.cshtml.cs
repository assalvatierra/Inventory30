using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebDBSchema.Models.Users;

namespace InvWeb.Pages.Masterfiles.Accounts
{
    public class MainModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public MainModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<AppUser> AppUsers { get; set; }

        public void OnGet()
        {
            var usersFromDb =  _context.Users.ToList();
        }
    }
}
