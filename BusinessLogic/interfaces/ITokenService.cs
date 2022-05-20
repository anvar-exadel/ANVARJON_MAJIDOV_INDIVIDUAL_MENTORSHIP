using Shared.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.interfaces
{
    public interface ITokenService
    {
        string GetToken(AppUser user);
    }
}
