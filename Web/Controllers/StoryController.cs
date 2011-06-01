using System.Net;
using System.Web.Mvc;
using Librarian.Logic.TinyPM;

namespace Librarian.Web.Controllers
{
    public class StoryController : Controller
    {
        readonly IApiClient apiClient;

        public StoryController(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        [Authorize]
        public ActionResult Backlog(int projectId)
        {
            var backlog = apiClient.GetBacklog(projectId);
            return View(backlog);
        }

        [Authorize]
        [HttpPost]
        public ActionResult SetColor(int storyId, string color)
        {
            return new HttpStatusCodeResult((int) HttpStatusCode.OK);
        }
    }
}