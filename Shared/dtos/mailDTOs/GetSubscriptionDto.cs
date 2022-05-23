using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.dtos.mailDTOs
{
    public class GetSubscriptionDto
    {
        public int Id { get; set; }
        public int Interval { get; set; }
        public List<string> Cities { get; set; }
        public int AppUserId { get; set; }
    }
}
