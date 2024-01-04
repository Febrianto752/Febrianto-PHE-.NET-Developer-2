using SupplyManagementSystem.Data;
using SupplyManagementSystem.Models;
using SupplyManagementSystem.Repositories.IRepositories;

namespace SupplyManagementSystem.Repositories
{
    public class VendorRepository : GeneralRepository<Vendor>, IVendorRepository
    {
        public VendorRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}