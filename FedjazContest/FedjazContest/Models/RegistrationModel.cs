using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FedjazContest.Models
{
    public class RegistrationModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter your First Name")]
        [StringLength(40, ErrorMessage = "Name is too big")]
        [RegularExpression("^[a-zA-Z .'-]+$", ErrorMessage = "First Name contains restricted characters")]
        public string FirstName { get; set; } = "";

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter your Last Name")]
        [StringLength(40, ErrorMessage = "Last Name is too big")]
        [RegularExpression("^[a-zA-Z .'-]+$", ErrorMessage = "Last Name contains restricted characters")]
        public string LastName { get; set; } = "";

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter your Username")]
        [StringLength(40, ErrorMessage = "Username is too big")]
        [RegularExpression("^[a-zA-Z0-9.'-_]+$", ErrorMessage = "Username contains restricted characters")]
        [Remote("CheckUsername", "Account", ErrorMessage = "Username is already in use", HttpMethod = "POST")]
        public string Username { get; set; } = "";

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter your Email")]
        [EmailAddress(ErrorMessage = "Email is invalid")]
        [Remote("CheckEmail", "Account", ErrorMessage = "Email is already in use", HttpMethod = "POST")]
        public string Email { get; set; } = "";

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter your Password")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Password length must be between 8 and 40 characters")]
        [Compare("PasswordConfirm", ErrorMessage = "Passwords must match")]
        public string Password { get; set; } = "";

        [Required(AllowEmptyStrings = false, ErrorMessage = "Confirm your Password")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Password length must be between 8 and 40 characters")]
        [Compare("Password", ErrorMessage = "Passwords must match")]
        public string PasswordConfirm { get; set; } = "";
        public IFormFile? Avatar { get; set; }
    }
}
