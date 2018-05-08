using System.Threading.Tasks;
using Project.Domain.SeedWork;

namespace Project.Domain.AggregatesModel
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<Project> GetAsync(int projectId);
        Task<Project> AddAsync(Project project);
        Task<Project> UpdateAsync(Project project);
    }
}