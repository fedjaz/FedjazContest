using System.ComponentModel.DataAnnotations;

namespace FedjazContest.Models.Settings
{
    public class RestorePasswordModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter your Email")]
        [EmailAddress(ErrorMessage = "Email is invalid")]
        public string Email { get; set; } = "";
    }
}
