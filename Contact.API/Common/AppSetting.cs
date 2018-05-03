using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Common
{
    public class AppSetting
    {
        public string MongoDbConnectionString { get; set; }
        public string MongoDbDatabase { get; set; }
    }
}
