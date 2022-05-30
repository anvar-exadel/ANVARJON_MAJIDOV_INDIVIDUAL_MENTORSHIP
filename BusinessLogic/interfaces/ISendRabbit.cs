using Shared.apiResponse.mailResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.interfaces
{
    public interface ISendRabbit
    {
        void SendMessage(string message, string queue);
    }
}
