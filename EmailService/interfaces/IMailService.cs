using Shared.apiResponse.mailResponse;

namespace EmailService.interfaces
{
    public interface IMailService
    {
        public void SendEmail(MailRequest mailer);
    }
}
