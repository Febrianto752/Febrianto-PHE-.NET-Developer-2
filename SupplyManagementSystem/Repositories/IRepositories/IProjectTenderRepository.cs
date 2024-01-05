using SupplyManagementSystem.Models;
using System.Collections.Generic;

namespace SupplyManagementSystem.Repositories.IRepositories
{
    public interface IProjectTenderRepository : IGeneralRepository<ProjectTender>
    {
        bool RemoveRange(IEnumerable<ProjectTender> projectsTenders);
    }
}
