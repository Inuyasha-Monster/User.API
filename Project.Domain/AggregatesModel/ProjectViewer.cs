using System;
using Project.Domain.SeedWork;

namespace Project.Domain.AggregatesModel
{
    public class ProjectViewer : Entity
    {
        public int ProjectId { get; set; }
        public string UserName { get; set; }
        public string Avator { get; set; }
        public int UserId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}