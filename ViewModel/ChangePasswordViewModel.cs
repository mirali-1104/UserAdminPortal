using System.ComponentModel.DataAnnotations;

namespace UserAdminPortal.ViewModel
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is Required.")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Compare("ConfirmNewPassword", ErrorMessage = "The password and confirmation password do not match.")]

        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is Required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        public string ConfirmNewPassword { get; set; }
        
    }
}
