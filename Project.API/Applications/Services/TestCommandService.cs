using System.Threading.Tasks;

namespace Project.API.Applications.Services
{
    public class TestCommandService : ICommandService
    {
        public async Task<bool> IsRecommandProject(int projectId, int userId)
        {
            return await Task.FromResult(true);
        }
    }
}