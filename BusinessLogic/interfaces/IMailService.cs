using Shared.apiResponse.serviceResponse;
using Shared.dtos.mailDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.interfaces
{
    public interface IMailService
    {
        ServiceResponse<GetSubscriptionDto> Subscribe(SubsribeUserDto subscribe);
        ServiceResponse<GetSubscriptionDto> Unsubscribe(int userId);
        ServiceResponse<string> GetReport(int userId, int requestTimeout);
    }
}
