using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReCommand.API.EFData.Entities
{
    public class ProjectReCommand
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FromUserId { get; set; }
        public string FromUserName { get; set; }
        public string FromUserAvator { get; set; }
        public EnumReCommandType EnumReCommandType { get; set; }
        public int ProjectId { get; set; }
        public string ProjectAvator { get; set; }
        public string Company { get; set; }
        public string Introduction { get; set; }
        public string Tags { get; set; }
        /// <summary>
        /// 融资阶段
        /// </summary>
        public string FinStage { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ReCommandTime { get; set; }
        public List<ProjectReferenceUser> ProjectReferenceUsers { get; set; }
    }
}
