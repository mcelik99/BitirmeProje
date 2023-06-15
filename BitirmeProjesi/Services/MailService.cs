using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;

namespace BitirmeProjesi.Services
{
    public class MailService
    {
        public void SendEmail(string emailAddress, string subject, string body)
        {
            string smtpHost = "smtp.gmail.com"; // SMTP sunucu adresi
            int smtpPort = 587; // SMTP sunucu portu
            string smtpUsername = "bitirmeornek@gmail.com"; // SMTP sunucu kullanıcı adı
            string smtpPassword = "rnloubgenjjjksax"; // SMTP sunucu şifresi

            using (var client = new SmtpClient(smtpHost, smtpPort))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                client.EnableSsl = true;

                using (var message = new MailMessage(smtpUsername, emailAddress))
                {
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = false;

                    client.Send(message);
                }
            }
        }
    }
}
