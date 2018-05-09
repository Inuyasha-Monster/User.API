using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Project.Infrastructure;

namespace Project.API.Applications.Queries
{
    public class ProjectQueries : IProjectQueries
    {
        private readonly string _connection;
        private readonly ProjectDbContext _dbContext;

        public ProjectQueries(string connection, ProjectDbContext dbContext)
        {
            _connection = connection;
            _dbContext = dbContext;
        }

        public async Task<dynamic> GetProjectListByUserIdAsync(int userId)
        {
            using (var connection = new MySqlConnection(_connection))
            {
                await connection.OpenAsync();
                string sql = @"SELECT a.*
                                FROM projects as a
                                WHERE a.UserId = @userId";
                var objects = await connection.QueryAsync(sql, new { userId });
                return objects;
            }
        }

        public async Task<dynamic> GetProjectDetailAsync(int projectId)
        {
            var project = await _dbContext.Projects
                .Include(x => x.ProjectContributors)
                .Include(x => x.ProjectPropetries)
                .Include(x => x.ProjectViewers)
                .Include(x => x.ProjectVisableRule)
                .SingleOrDefaultAsync(x => x.Id == projectId);
            return project;
        }
    }
}