using System.Threading.Tasks;
using Project.Domain.AggregatesModel;
using Project.Domain.SeedWork;

namespace Project.Infrastructure.Repositories
{
    public class ProjectRepository  : IProjectRepository
    {
        public IUnitOfWork UnitOfWork { get; }

        public async Task<Domain.AggregatesModel.Project> GetAsync(int projectId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Domain.AggregatesModel.Project> AddAsync(Domain.AggregatesModel.Project project)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Domain.AggregatesModel.Project> UpdateAsync(Domain.AggregatesModel.Project project)
        {
            throw new System.NotImplementedException();
        }
    }
}