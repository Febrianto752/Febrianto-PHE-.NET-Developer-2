using SupplyManagementSystem.Data;
using SupplyManagementSystem.Repositories;
using SupplyManagementSystem.Repositories.IRepositories;
using System;
using System.IO;
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

        [HttpGet]
        public ActionResult Details(Guid guid)
        {
            var vendor = _vendorRepository.Get(v => v.Guid == guid, includeProperties: "Account");

            if (vendor == null)
            {
                TempData["Error"] = "Data not found";
                return RedirectToAction("Index");
            }

            return View(vendor);
        }

        [HttpPost]
        public ActionResult Delete(Guid guid)
        {
            var vendor = _vendorRepository.Get(v => v.Guid == guid);
            var account = _accountRepository.Get(v => v.Guid == vendor.AccountGuid);

            if (vendor == null || account == null)
            {
                TempData["Error"] = "Data tidak ditemukan";
                return RedirectToAction("Index");
            }

            try
            {
                string uploadFolder = Server.MapPath("~/Static/Images");

                // Gabungkan path lengkap file gambar
                string imagePath = Path.Combine(uploadFolder, vendor.ProfileImage);

                if (System.IO.File.Exists(imagePath))
                {
                    // Hapus file jika ada
                    System.IO.File.Delete(imagePath);
                }
            }
            catch
            {

            }

            var deletedVendorIsSuccess = _vendorRepository.Delete(vendor);

            if (deletedVendorIsSuccess)
            {
                var deletedVendorAccountIsSuccess = _accountRepository.Delete(account);

                if (deletedVendorAccountIsSuccess)
                {
                    TempData["Success"] = "Data vendor berhasil dihapus";
                    return RedirectToAction("Index");
                }

            }
            TempData["Error"] = "Gagal menghapus data vendor";
            return RedirectToAction("Index");


        }
    }
}