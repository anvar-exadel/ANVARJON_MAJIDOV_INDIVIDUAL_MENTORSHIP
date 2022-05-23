using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.models.mail
{
    public class Subscription
    {
        public int Id { get; set; }
        public int Interval { get; set; }
        public List<City> Cities { get; set; }

        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
