using SupplyManagementSystem.Data;
using SupplyManagementSystem.Models;
using SupplyManagementSystem.Repositories;
using SupplyManagementSystem.Repositories.IRepositories;
using SupplyManagementSystem.Utilities;
using SupplyManagementSystem.Utilities.Enums;
using SupplyManagementSystem.ViewModels.Auth;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SupplyManagementSystem.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IVendorRepository _vendorRepository;
        private readonly ApplicationDbContext _db;

        public AuthController()
        {
            _db = new ApplicationDbContext();
            _accountRepository = new AccountRepository(_db);
            _vendorRepository = new VendorRepository(_db);
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVM loginVM)
        {
            var account = _accountRepository.Get(a => a.Email == loginVM.Email);

            if (account == null)
            {
                TempData["Error"] = "email atau password salah";
                return RedirectToAction("Login");
            }

            var validationPassword = HashingHandler.ValidatePassword(loginVM.Password, account.Password);

            if (validationPassword)
            {
                //Session["Username"] = account.Name;
                //Session["AccountGuid"] = account.Guid;
                //Session["Role"] = account.Role;
                // Membuat objek HttpCookie
                var username = new HttpCookie("Username");
                username.HttpOnly = true;

                var accountGuid = new HttpCookie("AccountGuid");
                accountGuid.HttpOnly = true;

                var role = new HttpCookie("Role");
                role.HttpOnly = true;


                // Menentukan nilai cookie
                username.Value = account.Name;
                accountGuid.Value = account.Guid.ToString();
                role.Value = account.Role;

                // Menentukan propertis lainnya seperti path, domain, expires, dll.
                username.Expires = DateTime.Now.AddHours(1);
                accountGuid.Expires = DateTime.Now.AddHours(1);
                role.Expires = DateTime.Now.AddHours(1);

                // Menambahkan username ke koleksi Response Cookies
                Response.Cookies.Add(username);
                Response.Cookies.Add(accountGuid);
                Response.Cookies.Add(role);
                FormsAuthentication.SetAuthCookie(account.Email, false);
                return RedirectToAction("Index", "Home");

            }

            TempData["Error"] = "email atau password salah";
            return View(loginVM);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            HttpCookie username = new HttpCookie("Username");
            username.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(username);

            HttpCookie accountGuid = new HttpCookie("AccountGuid");
            accountGuid.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(accountGuid);

            HttpCookie role = new HttpCookie("Role");
            role.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(role);

            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                var emailIsExist = _accountRepository.GetByEmail(registerVM.CreateAccountVM.Email);

                if (emailIsExist != null)
                {
                    TempData["Error"] = "Email sudah terdaftar";
                    return RedirectToAction(nameof(Register));
                }

                var newAccount = new Account()
                {
                    Guid = Guid.NewGuid(),
                    Name = registerVM.CreateAccountVM.Name,
                    Email = registerVM.CreateAccountVM.Email,
                    Password = HashingHandler.HashPassword(registerVM.CreateAccountVM.Password),
                    NoTelp = registerVM.CreateAccountVM.NoTelp,
                    Role = nameof(RoleEnum.Vendor),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                var transaction = _db.Database.BeginTransaction();

                var account = _accountRepository.Create(newAccount);

                if (account == null)
                {
                    TempData["Error"] = "Gagal membuat account";
                    transaction.Rollback();
                    return RedirectToAction(nameof(Register));
                }

                var newVendor = new Vendor()
                {
                    Guid = Guid.NewGuid(),
                    ProfileImage = "",
                    AccountGuid = account.Guid,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    Status = VendorStatusEnum.Pending,
                };
                var fileName = "";

                try
                {
                    if (registerVM.ProfileImage != null && registerVM.ProfileImage.ContentLength > 0)
                    {
                        // Mendapatkan ekstensi file
                        string fileExtension = Path.GetExtension(registerVM.ProfileImage.FileName).ToLower();

                        // Tentukan jenis file yang diperbolehkan (contoh: jpg, jpeg, png)
                        string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };

                        // Cek apakah ekstensi file yang diunggah diperbolehkan
                        if (Array.IndexOf(allowedExtensions, fileExtension) == -1)
                        {
                            TempData["Error"] = "Hanya file dengan ekstensi .jpg, .jpeg, atau .png yang diperbolehkan.";
                            transaction.Rollback();
                            return RedirectToAction(nameof(Register));
                        }

                        // Tentukan folder tempat menyimpan gambar
                        string uploadFolder = Server.MapPath("~/Static/Images/");
                        if (!Directory.Exists(uploadFolder))
                        {
                            Directory.CreateDirectory(uploadFolder);
                        }

                        // Mendapatkan nama unik untuk file
                        fileName = Guid.NewGuid().ToString() + fileExtension;

                        // Menyimpan gambar di server
                        registerVM.ProfileImage.SaveAs(Path.Combine(uploadFolder, fileName));

                        TempData["Success"] = "Gambar berhasil diunggah!";
                    }
                    else
                    {
                        TempData["Error"] = "Pilih gambar terlebih dahulu.";
                        transaction.Rollback();
                        return RedirectToAction(nameof(Register));
                    }
                }
                catch
                {
                    TempData["Error"] = "Gagal mengupload gambar";
                    transaction.Rollback();
                    return RedirectToAction(nameof(Register));
                }

                newVendor.ProfileImage = fileName;
                var vendor = _vendorRepository.Create(newVendor);

                if (vendor == null)
                {
                    TempData["Error"] = "Gagal membuat vendor";
                    transaction.Rollback();
                    return RedirectToAction(nameof(Register));
                }
                transaction.Commit();
                TempData["Success"] = "Berhasil membuat akun, silahkan login.";
                return RedirectToAction(nameof(Register));
            }

            return View(registerVM);
        }
    }
}