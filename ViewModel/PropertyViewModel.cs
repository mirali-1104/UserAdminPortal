using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UserAdminPortal.ViewModel
{
    public class PropertyViewModel
    {
        public int PropertyId { get; set; }

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

        public IFormFile ImageFile { get; set; } 
        public string ImagePath { get; set; } 
    }
}