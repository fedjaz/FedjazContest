using System.Net;
using System.Net.Mail;
using System.Text;

namespace FedjazContest.Services
{
    public class EmailService : IEmailService
    {
        string url;
        private readonly SmtpClient smtpClient;
        MailAddress from;
        private readonly IWebHostEnvironment environment;
        public EmailService(string email, string password, string smtpAddress, int port, string url, IServiceProvider serviceProvider)
        {
            this.url = url;
            from = new MailAddress(email);
            smtpClient = new SmtpClient(smtpAddress, port);
            smtpClient.Credentials = new NetworkCredential(email, password);
            smtpClient.EnableSsl = true;
            environment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
        }
        
        public async Task<string> ChangeEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<string> ChangePassword(string email)
        {
            return CreateRandomCode(32);
        }

        public async Task<string> ConfirmEmail(string email)
        {
            string code = CreateRandomCode(32);
            string url = string.Format(this.url, code);
            string body = string.Format(GetEmailFromFile("EmailConfirm.txt"), url);
            await SendEmail(email, "FedjazContest - Email confirmation", body);

            return code;
        }

        private string GetEmailFromFile(string name)
        {
            string path = Path.Combine(environment.ContentRootPath, "Emails", name);
            using(StreamReader reader = new StreamReader(path))
            {
                return reader.ReadToEnd();
            }
        }

        private string CreateRandomCode(int count)
        {
            List<char> chars = GetSymbols(true, true, true, true).ToList();
            StringBuilder stringBuilder = new StringBuilder();
            Random r = new Random();
            for(int i = 0; i < count; i++)
            {
                stringBuilder.Append(chars[r.Next(chars.Count)]);
            }

            return stringBuilder.ToString();
        }

        private async Task SendEmail(string email, string subject, string body)
        {
            MailAddress to = new MailAddress(email);
            MailMessage mailMessage = new MailMessage(from, to);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            await smtpClient.SendMailAsync(mailMessage);
        }

        private IEnumerable<char> GetSymbols(bool lowercase, bool uppercase, bool digits, bool symbols)
        {
            if (lowercase)
            {
                for(int i = 'a'; i <= 'z'; i++)
                {
                    yield return (char)i;
                }
            }

            if (uppercase)
            {
                for (int i = 'A'; i <= 'Z'; i++)
                {
                    yield return (char)i;
                }
            }

            if (digits)
            {
                for (int i = '0'; i <= '9'; i++)
                {
                    yield return (char)i;
                }
            }

            if (symbols)
            {
                yield return (char)'-';
                yield return (char)'_';
            }
        }
    }
}
