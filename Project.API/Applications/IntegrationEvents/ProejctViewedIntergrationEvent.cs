using System;

namespace Project.API.Applications.IntegrationEvents
{
    public class ProejctViewedIntergrationEvent : IntegrationEvent
    {
        public int ProjectId { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
    }
}