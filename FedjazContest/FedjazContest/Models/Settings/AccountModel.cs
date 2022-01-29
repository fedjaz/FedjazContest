using System.ComponentModel.DataAnnotations;

namespace FedjazContest.Models.Settings
{
    public class AccountModel
    {
        [StringLength(40, ErrorMessage = "First Name is too big")]
        [RegularExpression("^[a-zA-Z .'-]+$", ErrorMessage = "First Name contains restricted characters")]
        public string? FirstName { get; set; } = "";

        [StringLength(40, ErrorMessage = "Last Name is too big")]
        [RegularExpression("^[a-zA-Z .'-]+$", ErrorMessage = "First Name contains restricted characters")]
        public string? LastName { get; set; } = "";
    }
}
