using SupplyManagementSystem.Data;
using SupplyManagementSystem.Models;
using SupplyManagementSystem.Repositories.IRepositories;
using System.Linq;

namespace SupplyManagementSystem.Repositories
{
    public class AccountRepository : GeneralRepository<Account>, IAccountRepository
    {
        public AccountRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Account GetByEmail(string email)
        {
            return _context.Set<Account>().FirstOrDefault(e => e.Email == email);
        }
    }
}