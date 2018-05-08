using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Project.Domain.AggregatesModel;

namespace Project.API.Applications.Commands
{
    public class JoinProjectCommand : IRequest
    {
        public ProjectContributor ProjectContributor { get; set; }
    }
}
