using FedjazContest.Entities;

namespace FedjazContest.Models
{
    public class AccountComponentModel
    {
        public bool IsLogged { get; set; } = false;
        public string CallbackUrl { get; set; } = "/";
        public ApplicationUser? ApplicationUser { get; set; }

        public AccountComponentModel(bool isLogged, string callbackUrl)
        {
            IsLogged = isLogged;
            CallbackUrl = callbackUrl;
        }

        public AccountComponentModel(ApplicationUser applicationUser)
        {
            IsLogged = true;
            ApplicationUser = applicationUser;
        }
    }
}
