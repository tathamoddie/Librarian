using System.Web.Mvc;

namespace Librarian.Web.Controllers
{
    public class StoryController : Controller
    {
        public ActionResult Backlog(int projectId)
        {
            return View();
        }
    }
}