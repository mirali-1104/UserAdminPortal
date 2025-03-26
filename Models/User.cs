using Microsoft.AspNetCore.Identity;

namespace UserAdminPortal.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }

        public string Password { get; set; }
    }
}