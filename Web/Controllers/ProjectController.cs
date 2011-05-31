using System.Web.Mvc;

namespace Librarian.Web.Controllers
{
    public class ProjectController : Controller
    {
        public ActionResult ListProjects()
        {
            return View();
        }
    }
}