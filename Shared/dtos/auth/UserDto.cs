using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.dtos.auth
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}
