using EmailService.interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Shared.apiResponse.mailResponse;
using Shared.configFiles;
using System;

namespace EmailService.services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public void SendEmail(MailRequest mailer)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
            message.To.Add(new MailboxAddress(mailer.ToEmail, mailer.ToEmail));
            message.Subject = mailer.Subject;

            message.Body = new TextPart("plain") { Text = mailer.Body };
            try
            {
                using SmtpClient client = new SmtpClient();
                client.Connect(_mailSettings.Host, _mailSettings.Port, true);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(_mailSettings.Mail, _mailSettings.Password);

                client.Send(message);
                client.Disconnect(true);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
