using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserAdminPortal.Models;

namespace UserAdminPortal.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Admin> Admins { get; set; }
        //public DbSet<User> Users { get; set; }
    }
}
