using Project.Domain.SeedWork;

namespace Project.Domain.AggregatesModel
{
    public class ProjectVisableRule : Entity
    {
        public int ProjectId { get; set; }
        public bool Visable { get; set; }
        public string Tags { get; set; }
    }
}