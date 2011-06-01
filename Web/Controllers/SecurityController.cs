using System.Web.Mvc;
using System.Web.Security;
using Librarian.Logic.TinyPM;
using Librarian.Web.Services;

namespace Librarian.Web.Controllers
{
    public class SecurityController : Controller
    {
        readonly ISecurityService securityService;

        public SecurityController(ISecurityService securityService)
        {
            this.securityService = securityService;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new ApiCredential());
        }

        [HttpPost]
        public ActionResult Login(ApiCredential credentials, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(credentials);

            var isLoginValid = securityService.Authenticate(credentials, Response);

            if (isLoginValid)
            {
                return Url.IsLocalUrl(returnUrl)
                    ? (ActionResult)Redirect(returnUrl)
                    : RedirectToRoute(RouteNames.ListProjects);
            }

            ModelState.AddModelError("login-failed", "The login failed.");
            return View(credentials);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToRoute(RouteNames.Home);
        }
    }
}