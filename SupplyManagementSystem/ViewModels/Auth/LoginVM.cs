﻿using System.ComponentModel.DataAnnotations;

namespace SupplyManagementSystem.ViewModels.Auth
{
    public class LoginVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}