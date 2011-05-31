using System.Web.Mvc;
using Librarian.Web.Models;
using Librarian.Web.Services;

namespace Librarian.Web.Controllers
{
    public class SecurityController : Controller
    {
        readonly ISecurityService securityService;

        public SecurityController()
            : this(new SecurityService())
        {}

        public SecurityController(ISecurityService securityService)
        {
            this.securityService = securityService;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new LoginCredentials());
        }

        [HttpPost]
        public ActionResult Login(LoginCredentials credentials, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(credentials);

            var isLoginValid = securityService.Authenticate(credentials);

            if (isLoginValid)
            {
                return Url.IsLocalUrl(returnUrl)
                    ? (ActionResult)Redirect(returnUrl)
                    : RedirectToRoute(RouteNames.ListProjects);
            }

            ModelState.AddModelError("login-failed", "The login failed.");
            return View(credentials);
        }
    }
}