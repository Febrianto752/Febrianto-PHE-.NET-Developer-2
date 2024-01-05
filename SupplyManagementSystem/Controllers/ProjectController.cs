using SupplyManagementSystem.Data;
using SupplyManagementSystem.Repositories;
using SupplyManagementSystem.Repositories.IRepositories;
using System.Linq;
using System.Web.Mvc;

namespace SupplyManagementSystem.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IVendorRepository _vendorRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectTenderRepository _projectTenderRepository;
        private readonly ApplicationDbContext _db;

        public ProjectController()
        {
            _db = new ApplicationDbContext();
            _accountRepository = new AccountRepository(_db);
            _vendorRepository = new VendorRepository(_db);
            _projectRepository = new ProjectRepository(_db);
            _projectTenderRepository = new ProjectTenderRepository(_db);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var projects = _projectRepository.GetAll().ToList();
            return View(projects);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
    }
}