using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Models
{
    public class BPFile
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public string FileName { get; set; }
        public string OriginFilePath { get; set; }
        public string FormatFilePath { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
