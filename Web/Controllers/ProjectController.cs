using System.Web;
using System.Web.Mvc;
using Librarian.Logic.TinyPM;
using Librarian.Web.Services;

namespace Librarian.Web.Controllers
{
    public class ProjectController : Controller
    {
        readonly IApiClient apiClient;

        public ProjectController()
            : this(new HttpContextWrapper(System.Web.HttpContext.Current))
        {}

        public ProjectController(HttpContextBase httpContext)
            : this(new ApiClient(new ApiCredentialProvider(httpContext)))
        {}

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