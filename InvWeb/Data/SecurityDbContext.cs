using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InvWeb.Data
{
    public class SecurityDbContext: IdentityDbContext
    {
        public SecurityDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

    }
}
