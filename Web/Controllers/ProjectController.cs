using System.Web.Mvc;
using Librarian.Logic.TinyPM;

namespace Librarian.Web.Controllers
{
    public class ProjectController : Controller
    {
        readonly IApiClient apiClient;

        public ProjectController(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        [Authorize]
        public ActionResult ListProjects()
        {
            return View();
        }
    }
}