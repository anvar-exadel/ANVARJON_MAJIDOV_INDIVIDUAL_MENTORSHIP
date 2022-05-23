using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.models.mail
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }
    }
}
