using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Applications.IntegrationEvents
{
    public class ProjectCreatedIntergrationEvent : IntegrationEvent
    {
        public int UserId { get; set; }
        public string Company { get; set; }
        public int ProjectId { get; set; }
        public string UserName { get; set; }
    }
}
