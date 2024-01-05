using SupplyManagementSystem.Data;
using SupplyManagementSystem.Repositories;
using SupplyManagementSystem.Repositories.IRepositories;
using System.Web.Mvc;

namespace SupplyManagementSystem.Controllers
{
    public class VendorController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IVendorRepository _vendorRepository;
        private readonly ApplicationDbContext _db;

        public VendorController()
        {
            _db = new ApplicationDbContext();
            _accountRepository = new AccountRepository(_db);
            _vendorRepository = new VendorRepository(_db);
        }

        public ActionResult Index()
        {
            var vendors = _vendorRepository.GetAll(includeProperties: "Account");
            return View(vendors);
        }
    }
}