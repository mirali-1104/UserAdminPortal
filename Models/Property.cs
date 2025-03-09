using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAdminPortal.Models
{
    public class Property
    {
        [Key]
        public int PropertyId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [StringLength(255)]
        public string PropertyName { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        public string Image { get; set; } 

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}