using System.Threading.Tasks;

namespace Project.API.Applications.Services
{
    public interface ICommandService
    {
        Task<bool> IsRecommandProject(int projectId, int userId);
    }
}