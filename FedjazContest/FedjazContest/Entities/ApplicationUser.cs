using Microsoft.AspNetCore.Identity;

namespace FedjazContest.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName {  get; set; } = "";
        public string LastName {  get; set; } = "";
        public int ImageId { get; set; }
        public string EmailConfirmationCode { get; set; } = "";
    }
}
