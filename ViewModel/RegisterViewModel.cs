using System.ComponentModel.DataAnnotations;

namespace UserAdminPortal.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Name is Required.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is Required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [Compare("ConfirmPassword", ErrorMessage = "The password and confirmation password do not match.")]

        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is Required.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string ConfirmPasswordConfirmation { get;set; }

        public string UserName { get; set; }
    }
}
