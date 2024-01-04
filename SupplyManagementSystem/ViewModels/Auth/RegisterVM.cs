using SupplyManagementSystem.ViewModels.Account;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace SupplyManagementSystem.ViewModels.Auth
{
    public class RegisterVM
    {
        [Required]
        public HttpPostedFileBase ProfileImage { get; set; }

        public CreateAccountVM CreateAccountVM { get; set; }
    }
}