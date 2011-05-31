using System.Web.Mvc;

namespace Librarian.Web.Controllers
{
    public class ProjectController : Controller
    {
        [Authorize]
        public ActionResult ListProjects()
        {
            return View();
        }
    }
}