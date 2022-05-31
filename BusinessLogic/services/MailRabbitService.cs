using BusinessLogic.interfaces;
using Shared.apiResponse.mailResponse;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BusinessLogic.services
{
    public class MailRabbitService : IMailService
    {
        private readonly ISendRabbit _sendRabbit;

        public MailRabbitService(ISendRabbit sendRabbit)
        {
            _sendRabbit = sendRabbit;
        }

        public void SendEmail(MailRequest mail)
        {
            string message = JsonSerializer.Serialize<MailRequest>(mail);
            _sendRabbit.SendMessage(message, "emailQueue");
        }
    }
}
