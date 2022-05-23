using Shared.models.mail;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public UserRole UserRole { get; set; }
        public Subscription Subscription { get; set; }
    }
}
