using SupplyManagementSystem.Data;
using SupplyManagementSystem.Models;
using SupplyManagementSystem.Repositories;
using SupplyManagementSystem.Repositories.IRepositories;
using SupplyManagementSystem.ViewModels.Project;
using System;
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

        [HttpPost]
        public ActionResult Create(CreateProjectVM createProjectVM)
        {
            if (ModelState.IsValid)
            {
                if (createProjectVM.EndDate < createProjectVM.StartDate)
                {
                    TempData["Error"] = "Tanggal selesai tidak boleh kurang dari tanggal mulai";
                    return View(createProjectVM);
                }

                var newProject = new Project()
                {
                    Guid = Guid.NewGuid(),
                    Name = createProjectVM.Name,
                    Description = createProjectVM.Description,
                    StartDate = createProjectVM.StartDate,
                    EndDate = createProjectVM.EndDate,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                var createdProjectIsSuccess = _projectRepository.Create(newProject);

                if (createdProjectIsSuccess != null)
                {
                    TempData["Success"] = "Berhasil membuat project";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Gagal membuat project";
                    return RedirectToAction("Index");
                }


            }

            return View(createProjectVM);
        }
    }
}