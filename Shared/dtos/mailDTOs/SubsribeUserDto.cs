using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.dtos.mailDTOs
{
    public class SubsribeUserDto
    {
        public int UserId { get; set; }
        public int IntervalInHours { get; set; }
        public List<string> Cities { get; set; }
    }
}
