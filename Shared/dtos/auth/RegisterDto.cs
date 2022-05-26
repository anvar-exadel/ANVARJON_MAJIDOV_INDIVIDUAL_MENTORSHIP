using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.dtos.auth
{
    public class RegisterDto
    {
        public string UserName { get; set; }
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
