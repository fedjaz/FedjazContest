namespace FedjazContest.Models
{
    public class AccountComponentModel
    {
        public bool IsLogged { get; set; } = false;
        public string CallbackUrl { get; set; } = "/";
        //public User User { get; set; }

        public AccountComponentModel(bool isLogged, string callbackUrl)
        {
            IsLogged = isLogged;
            CallbackUrl = callbackUrl;
        }
    }
}
