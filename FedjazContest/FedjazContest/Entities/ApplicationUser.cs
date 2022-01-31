using Microsoft.AspNetCore.Identity;

namespace FedjazContest.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public int ImageId { get; set; }
        public string EmailConfirmationCode { get; set; } = "";
        public string PasswordChangeCode { get; set; } = "";
        public string Bio { get; set; } = "";
        public int Score { get; set; } = 0;
        public int TasksSolved { get; set; } = 0;
        public string PreferredLanguage { get; set; } = "";
        public string PreferredCompiler { get; set; } = "";
    }
}
