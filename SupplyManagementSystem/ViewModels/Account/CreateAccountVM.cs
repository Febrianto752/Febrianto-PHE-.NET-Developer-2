using System.ComponentModel.DataAnnotations;

namespace SupplyManagementSystem.ViewModels.Account
{
    public class CreateAccountVM
    {
        [EmailAddress, Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string NoTelp { get; set; }
    }
}