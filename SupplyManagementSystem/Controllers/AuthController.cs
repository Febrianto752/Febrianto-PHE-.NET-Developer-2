using System.Web.Mvc;

namespace SupplyManagementSystem.Controllers
{
    public class AuthController : Controller
    {

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
    }
}