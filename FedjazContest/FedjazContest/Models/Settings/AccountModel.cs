using System.ComponentModel.DataAnnotations;

namespace FedjazContest.Models.Settings
{
    public class AccountModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter your First Name")]
        [StringLength(40, ErrorMessage = "First Name is too big")]
        [RegularExpression("^[a-zA-Z .'-]+$", ErrorMessage = "First Name contains restricted characters")]
        public string FirstName { get; set; } = "";

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter your Last Name")]
        [StringLength(40, ErrorMessage = "Last Name is too big")]
        [RegularExpression("^[a-zA-Z .'-]+$", ErrorMessage = "Last Name contains restricted characters")]
        public string LastName { get; set; } = "";

        [StringLength(1000, ErrorMessage = "Bio is too big")]
        [RegularExpression(@"^[a-zA-Zа-яА-Я0-9 .,'\-!\(\)]+$", ErrorMessage = "Bio contains restricted characters")]
        public string? Bio { get; set; }
        public IFormFile? Avatar { get; set; }

        public AccountModel()
        {
        }

        public AccountModel(string firstName, string lastName, string? bio)
        {
            FirstName = firstName;
            LastName = lastName;
            Bio = bio;
        }
    }
}
