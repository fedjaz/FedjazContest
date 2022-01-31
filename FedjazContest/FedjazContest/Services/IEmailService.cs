namespace FedjazContest.Services
{
    public interface IEmailService
    {
        public Task<string> ConfirmEmail(string email);
        public Task<string> ChangePassword(string email);
        public Task<string> ChangeEmail(string email);
    }
}
