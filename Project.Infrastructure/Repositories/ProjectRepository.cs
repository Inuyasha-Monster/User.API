using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Domain.AggregatesModel;
using Project.Domain.SeedWork;

namespace Project.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ProjectDbContext _dbContext;

        public ProjectRepository(ProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IUnitOfWork UnitOfWork => _dbContext;

        public async Task<Domain.AggregatesModel.Project> GetAsync(int projectId)
        {
            return await _dbContext.Projects.Include(x => x.ProjectContributors)
                .Include(x => x.ProjectPropetries)
                .Include(x => x.ProjectViewers)
                .SingleOrDefaultAsync(x => x.Id == projectId);
        }

        public async Task AddAsync(Domain.AggregatesModel.Project project)
        {
            await _dbContext.Projects.AddAsync(project);
        }

        public async Task UpdateAsync(Domain.AggregatesModel.Project project)
        {
            _dbContext.Update(project);
            await Task.CompletedTask;
        }
    }
}