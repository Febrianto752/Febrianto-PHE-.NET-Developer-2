using System.Web.Mvc;

namespace SupplyManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }



    }
}