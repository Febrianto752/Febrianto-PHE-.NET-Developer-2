using SupplyManagementSystem.Data;
using SupplyManagementSystem.Models;
using SupplyManagementSystem.Repositories.IRepositories;

namespace SupplyManagementSystem.Repositories
{
    public class ProjectRepository : GeneralRepository<Project>, IProjectRepository
    {
        public ProjectRepository(ApplicationDbContext context) : base(context)
        {
        }


    }
}