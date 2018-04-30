using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Models
{
    public class AppUserTag
    {
        public int AppUserId { get; set; }
        public string Tag { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
