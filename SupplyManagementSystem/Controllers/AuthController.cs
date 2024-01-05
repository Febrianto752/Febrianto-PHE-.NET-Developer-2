﻿using SupplyManagementSystem.Data;
using SupplyManagementSystem.Models;
using SupplyManagementSystem.Repositories;
using SupplyManagementSystem.Repositories.IRepositories;
using SupplyManagementSystem.Utilities;
using SupplyManagementSystem.Utilities.Enums;
using SupplyManagementSystem.ViewModels.Auth;
using System;
using System.IO;
using System.Web.Mvc;

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


        public ActionResult Login()
        {
            return View();
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