namespace FedjazContest.Services
{
    public interface IEmailService
    {
        public string CreateRandomCode(int count);
        public Task ConfirmEmail(string email, string code);
        public Task ChangePasswod(string email, string code);
        public Task ChangeEmail(string email, string code);
    }
}
