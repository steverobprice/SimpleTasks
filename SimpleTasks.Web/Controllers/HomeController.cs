using System.Web.Mvc;

namespace SimpleTasks.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}