using Shared.apiResponse.mailResponse;
using Shared.apiResponse.serviceResponse;
using Shared.dtos.mailDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.interfaces
{
    public interface IMailSubService
    {
        ServiceResponse<GetSubscriptionDto> Subscribe(SubsribeUserDto subscribe, int requestTimeout);
        ServiceResponse<GetSubscriptionDto> Unsubscribe(int userId);
        ServiceResponse<string> GetReport(int userId, int requestTimeout);
    }
}
