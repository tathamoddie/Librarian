using System;
using System.Web.Mvc;
using Librarian.Web.Models;

namespace Librarian.Web.Controllers
{
    public class SecurityController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View(new LoginCredentials());
        }

        [HttpPost]
        public ActionResult Login(LoginCredentials credentials)
        {
            if (!ModelState.IsValid)
                return View(credentials);

            throw new NotImplementedException();
        }
    }
}