using System.ComponentModel.DataAnnotations;

namespace UserAdminPortal.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }  // Primary Key
        [Required]
        public string Name { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

}
