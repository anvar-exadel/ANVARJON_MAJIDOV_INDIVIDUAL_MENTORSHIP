using BusinessLogic.models;
using Shared.dtos.auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.interfaces
{
    public interface IAuthService
    {
        ServiceResponse<UserDto> Register(RegisterDto registerDto);
        ServiceResponse<UserDto> Login(LoginDto loginDto);
    }
}
