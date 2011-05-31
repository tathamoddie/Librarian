using System.Web.Mvc;

namespace Librarian.Web.Controllers
{
    public class SecurityController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
    }
}