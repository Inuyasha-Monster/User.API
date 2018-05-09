namespace Project.API.Applications.IntegrationEvents
{
    public class ProjectJoinedIntergrationEvent : IntegrationEvent
    {
        public int ProjectId { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
    }
}