using SupplyManagementSystem.Data;
using SupplyManagementSystem.Repositories;
using SupplyManagementSystem.Repositories.IRepositories;
using SupplyManagementSystem.Utilities.Enums;
using SupplyManagementSystem.ViewModels.Vendor;
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

        [HttpPost]
        public ActionResult ApproveByAdmin(Guid guid)
        {
            var vendor = _vendorRepository.Get(v => v.Guid == guid, includeProperties: "Account");

            if (vendor == null)
            {
                TempData["Error"] = "Data Not Found";
            }

            TempData["Success"] = $"Vendor {vendor.Account.Name} disetujui bergabung oleh admin";
            vendor.Status = VendorStatusEnum.ApproveByAdmin;
            _vendorRepository.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ApproveByManager(Guid guid)
        {
            var vendor = _vendorRepository.Get(v => v.Guid == guid, includeProperties: "Account");

            if (vendor == null)
            {
                TempData["Error"] = "Data Not Found";
            }

            TempData["Success"] = $"Vendor {vendor.Account.Name} disetujui bergabung oleh Manager";
            vendor.Status = VendorStatusEnum.Accepted;
            _vendorRepository.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            var email = User.Identity.Name;
            var vendorAccount = _accountRepository.Get(a => a.Email == email);

            if (vendorAccount == null)
            {
                TempData["Error"] = "Data tidak ditemukan";
                return RedirectToAction("Index", "Home");
            }

            var vendor = _vendorRepository.Get(v => v.AccountGuid == vendorAccount.Guid);

            if (vendor == null)
            {
                TempData["Error"] = "Data tidak ditemukan";
                return RedirectToAction("Index", "Home");
            }

            var vendorProfile = new EditProfileVendorVM()
            {
                BusinessField = vendor.BusinessField,
                TypeCompany = vendor.TypeCompany,
                VendorGuid = vendor.Guid,
                Status = vendor.Status,
                VendorName = vendorAccount.Name
            };

            return View(vendorProfile);
        }

        [HttpPost]
        public ActionResult EditProfile(EditProfileVendorVM editProfileVendorVM)
        {
            if (ModelState.IsValid)
            {
                var vendor = _vendorRepository.Get(v => v.Guid == editProfileVendorVM.VendorGuid);
                if (vendor == null)
                {
                    TempData["Error"] = "data tidak ditemukan";
                    return RedirectToAction("Index", "Home");
                }

                try
                {
                    vendor.BusinessField = editProfileVendorVM.BusinessField ?? "";
                    vendor.TypeCompany = editProfileVendorVM.TypeCompany ?? "";
                    vendor.ModifiedDate = DateTime.Now;

                    _vendorRepository.SaveChanges();

                    var accountVendor = _accountRepository.Get(a => a.Guid == vendor.AccountGuid);

                    accountVendor.Name = editProfileVendorVM.VendorName;
                    _accountRepository.SaveChanges();

                    TempData["Success"] = "Berhasil mengubah data profile";
                    return RedirectToAction("EditProfile", "Vendor");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Terjadi kesalahan di server";
                    return RedirectToAction("EditProfile", "Vendor");

                }
            }

            return View(editProfileVendorVM);
        }
    }
}