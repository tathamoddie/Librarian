using System.Web;
using System.Web.Security;
using FormsAuthenticationExtensions;
using Librarian.Logic.TinyPM;

namespace Librarian.Web.Services
{
    public class ApiCredentialProvider : IApiCredentialProvider
    {
        readonly ApiCredential credential;

        public ApiCredentialProvider(HttpContextBase context)
        {
            var identity = context.User.Identity as FormsIdentity;

            var userData = identity != null && identity.IsAuthenticated
                ? identity.Ticket.GetStructuredUserData()
                : null;

            credential = userData != null
                ? ApiCredential.FromNameValueCollection(userData)
                : null;
        }

        public ApiCredential Credentials
        {
            get { return credential; }
        }
    }
}