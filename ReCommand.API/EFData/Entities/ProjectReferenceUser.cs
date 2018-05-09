using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReCommand.API.EFData.Entities
{
    public class ProjectReferenceUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
