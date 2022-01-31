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
        public EmailService(string email, string password, string smtpAddress, int port, string url)
        {
            this.url = url;
            from = new MailAddress(email);
            smtpClient = new SmtpClient(smtpAddress, port);
            smtpClient.Credentials = new NetworkCredential(email, password);
            smtpClient.EnableSsl = true;
        }
        
        public async Task ChangeEmail(string email, string code)
        {
            throw new NotImplementedException();
        }

        public async Task ChangePasswod(string email, string code)
        {
            throw new NotImplementedException();
        }

        public async Task ConfirmEmail(string email, string code)
        {
            throw new NotImplementedException();
        }

        public string CreateRandomCode(int count)
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
