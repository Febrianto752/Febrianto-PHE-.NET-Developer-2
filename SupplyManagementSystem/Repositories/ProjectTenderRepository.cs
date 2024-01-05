using SupplyManagementSystem.Data;
using SupplyManagementSystem.Models;
using SupplyManagementSystem.Repositories.IRepositories;
using System.Collections.Generic;

namespace SupplyManagementSystem.Repositories
{
    public class ProjectTenderRepository : GeneralRepository<ProjectTender>, IProjectTenderRepository
    {
        public ProjectTenderRepository(ApplicationDbContext context) : base(context) { }

        public bool RemoveRange(IEnumerable<ProjectTender> projectTenders)
        {
            try
            {
                _context.Set<ProjectTender>().RemoveRange(projectTenders);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}