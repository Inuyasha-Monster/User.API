namespace ReCommand.API.IntergationEvents
{
    public class ProjectCreatedIntergrationEvent : IntegrationEvent
    {
        public int UserId { get; set; }
        public string Company { get; set; }
        public int ProjectId { get; set; }
        public string Introduction { get; set; }
    }
}
