using SupplyManagementSystem.Data;
using SupplyManagementSystem.Models;
using SupplyManagementSystem.Repositories;
using SupplyManagementSystem.Repositories.IRepositories;
using SupplyManagementSystem.ViewModels.Project;
using SupplyManagementSystem.ViewModels.ProjectTender;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SupplyManagementSystem.Controllers
{
    [Authorize]
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

        [HttpGet]
        public ActionResult Details(Guid guid)
        {
            var project = _projectRepository.Get(v => v.Guid == guid);


            if (project == null)
            {
                TempData["Error"] = "Data not found";
                return RedirectToAction("Index");
            }

            var projectDetails = new ProjectDetailsVM();

            projectDetails.ProjectGuid = project.Guid;
            projectDetails.ProjectName = project.Name;
            projectDetails.Description = project.Description;
            projectDetails.Description = project.Description;
            projectDetails.StartDate = project.StartDate;
            projectDetails.EndDate = project.EndDate;


            var tendersByProject = _projectTenderRepository.GetAll().Where(pt => pt.ProjectGuid == project.Guid).ToList();

            if (tendersByProject != null)
            {
                var vendors = _vendorRepository.GetAll();
                var accounts = _accountRepository.GetAll();

                projectDetails.VendorParticipants = tendersByProject.Select(tp =>
                {

                    var vendor = vendors.FirstOrDefault(v => v.Guid == tp.VendorGuid);
                    var vendorAccount = accounts.FirstOrDefault(a => a.Guid == vendor.AccountGuid);

                    var vendorParticiapnt = new VendorParticipantVM()
                    {
                        ProjectTenderGuid = tp.Guid,
                        VendorName = vendorAccount.Name,
                        BusinessField = vendor.BusinessField,
                        TypeCompany = vendor.TypeCompany,

                    };

                    return vendorParticiapnt;


                }).ToList();
            }

            return View(projectDetails);
        }

        [HttpGet]
        public ActionResult Edit(Guid guid)
        {
            var project = _projectRepository.Get(p => p.Guid == guid);

            if (project == null)
            {
                TempData["Error"] = "Data not found";
                return RedirectToAction("Index");
            }
            //project.StartDate = project.StartDate.ToString()

            return View(project);

        }

        [HttpPost]
        public ActionResult Edit(EditProjectVM editProjectVM)
        {
            if (ModelState.IsValid)
            {
                var project = _projectRepository.Get(p => p.Guid == editProjectVM.Guid);

                if (project == null)
                {
                    TempData["Error"] = "Data tidak ditemukan";
                    return RedirectToAction("Index");
                }

                project.Name = editProjectVM.Name;
                project.Description = editProjectVM.Description;
                project.StartDate = editProjectVM.StartDate;
                project.EndDate = editProjectVM.EndDate;

                _projectRepository.SaveChanges();

                TempData["Success"] = "Berhasil mengubah data project";
                return RedirectToAction("Index");
            }

            return View(editProjectVM);
        }
    }
}