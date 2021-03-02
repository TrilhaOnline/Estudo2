using System.Web.Mvc;

namespace DC.Application.Api.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return RedirectToAction("", "swagger");
        }
    }
}