using SupplyManagementSystem.Data;
using SupplyManagementSystem.Models;
using SupplyManagementSystem.Repositories;
using SupplyManagementSystem.Repositories.IRepositories;
using SupplyManagementSystem.Utilities.Enums;
using SupplyManagementSystem.ViewModels.ProjectTender;
using System;
using System.Web.Mvc;

namespace SupplyManagementSystem.Controllers
{
    public class ProjectTenderController : Controller
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectTenderRepository _projectTenderRepository;
        private readonly ApplicationDbContext _db;

        public ProjectTenderController()
        {
            _db = new ApplicationDbContext();
            _vendorRepository = new VendorRepository(_db);
            _projectRepository = new ProjectRepository(_db);
            _projectTenderRepository = new ProjectTenderRepository(_db);
        }

        [HttpPost]
        public ActionResult Create(ProjectTenderVM projectTenderVM)
        {
            var projectTenderIsExist = _projectTenderRepository.Get(pt => pt.ProjectGuid == projectTenderVM.ProjectGuid && pt.VendorGuid == projectTenderVM.VendorGuid);

            if (projectTenderIsExist != null)
            {
                TempData["Error"] = "anda sudah mengikuti project ini";
                return RedirectToAction("Index", "Project");
            }

            var project = _projectRepository.Get(p => p.Guid == projectTenderVM.ProjectGuid);

            if (project == null)
            {
                TempData["Error"] = "Data project tidak ditemukan";
                return RedirectToAction("Index", "Project");
            }

            var vendor = _vendorRepository.Get(v => v.Guid == projectTenderVM.VendorGuid);

            if (vendor == null)
            {
                TempData["Error"] = "Data vendor tidak ditemukan";
                return RedirectToAction("Index", "Project");
            }

            //var projectTender = new ProjectTender()
            //{
            //    Guid = Guid.NewGuid(),
            //    ProjectGuid = project.Guid,
            //    VendorGuid = vendor.Guid,
            //    Status = TenderStatusEnum.Submitted,
            //    CreatedDate = DateTime.Now,
            //    ModifiedDate = DateTime.Now,
            //};

            var projectTender = new ProjectTender()
            {
                Guid = Guid.NewGuid(),
                ProjectGuid = project.Guid,
                VendorGuid = vendor.Guid,
                Status = TenderStatusEnum.Submitted,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            };



            var createdProjectTender = _projectTenderRepository.Create(projectTender);
            //_projectTenderRepository.SaveChanges();
            if (createdProjectTender == null)
            {
                TempData["Error"] = "Terjadi kesalahan diserver";
                return RedirectToAction("Index", "Project");
            }

            TempData["Success"] = "Berhasil mengikuti project";
            return RedirectToAction("Index", "Project");

        }

        [HttpPost]
        public ActionResult Delete(ProjectTenderVM projectTenderVM)
        {
            var projectTender = _projectTenderRepository.Get(pt => pt.VendorGuid == projectTenderVM.VendorGuid && pt.ProjectGuid == projectTenderVM.ProjectGuid);

            if (projectTender == null)
            {
                TempData["Error"] = "Data project tender tidak ditemukan";
                return RedirectToAction("Index", "Project");
            }

            _projectTenderRepository.Delete(projectTender);
            TempData["Success"] = "Berhasil membatalkan keikutsertaan dalam project";
            return RedirectToAction("Index", "Project");

        }
    }
}