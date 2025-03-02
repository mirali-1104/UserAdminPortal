using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace UserAdminPortal.Models
{
    [Index(nameof(UserName), IsUnique = true)] 
    [Index(nameof(PhoneNumber), IsUnique = true)] 
    public class User
    {
        [Key] 
        public Guid Id { get; set; } = Guid.NewGuid();

        public string UserName { get; set; } 
        [Required]
        public string NormalizedUserName { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; } 
        public string PhoneNumber { get; set; } 
        [Required]
        public string Role { get; set; }
        [Required]
        public string SecurityStamp { get; set; }
        public bool IsEmailConfirmed { get; set; }
        [Required]
        public bool IsNumberConfirmed { get; set; }
        public string ProfileImage { get; set; }
    }
}