using SupplyManagementSystem.Models;

namespace SupplyManagementSystem.Repositories.IRepositories
{
    public interface IAccountRepository : IGeneralRepository<Account>
    {
        Account GetByEmail(string email);
    }
}
