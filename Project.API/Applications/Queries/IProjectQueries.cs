using System.Threading.Tasks;

namespace Project.API.Applications.Queries
{
    public interface IProjectQueries
    {
        Task<dynamic> GetProjectListByUserIdAsync(int userId);
        Task<dynamic> GetProjectDetailAsync(int projectId);
    }
}