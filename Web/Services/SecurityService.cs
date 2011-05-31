using System.Web;
using System.Web.Security;
using FormsAuthenticationExtensions;
using Librarian.Web.Models;

namespace Librarian.Web.Services
{
    public class SecurityService : ISecurityService
    {
        public bool Authenticate(LoginCredentials credentials, HttpResponseBase response)
        {
            var userData = credentials.ToNameValueCollection();
            new FormsAuthentication().SetAuthCookie(response, "tinyPM User", true, userData);

            return true;
        }
    }
}