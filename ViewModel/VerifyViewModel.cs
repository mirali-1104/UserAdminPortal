using System.ComponentModel.DataAnnotations;

namespace UserAdminPortal.ViewModel
{
    public class VerifyViewModel
    {
        [Required(ErrorMessage ="Email is Required")]
        [EmailAddress]
        public string Email { get; set; }
       
    }
}
