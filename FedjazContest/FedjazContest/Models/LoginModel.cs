using System.ComponentModel.DataAnnotations;

namespace FedjazContest.Models
{
    public class LoginModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter your Email or Username")]
        public string EmailOrUsername { get; set; } = "";

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter your Password")]
        public string Password { get; set; } = "";
    }
}
