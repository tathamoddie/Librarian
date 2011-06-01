using System.Web;
using System.Web.Security;
using FormsAuthenticationExtensions;
using Librarian.Logic.TinyPM;

namespace Librarian.Web.Services
{
    public class SecurityService : ISecurityService
    {
        public bool Authenticate(ApiCredential credentials, HttpResponseBase response)
        {
            var userData = credentials.ToNameValueCollection();
            new FormsAuthentication().SetAuthCookie(response, "tinyPM User", true, userData);

            return true;
        }
    }
}