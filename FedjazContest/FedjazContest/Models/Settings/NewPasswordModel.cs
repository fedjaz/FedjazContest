using System.ComponentModel.DataAnnotations;

namespace FedjazContest.Models.Settings
{
    public class NewPasswordModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter your Password")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Password length must be between 8 and 40 characters")]
        public string Password { get; set; } = "";

        [Required(AllowEmptyStrings = false, ErrorMessage = "Confirm your Password")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Password length must be between 8 and 40 characters")]
        [Compare("Password", ErrorMessage = "Passwords must match")]
        public string PasswordConfirm { get; set; } = "";
        public string Code { get; set; } = "";
    }
}
