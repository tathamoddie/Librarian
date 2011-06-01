using System.Web;
using Librarian.Logic.TinyPM;

namespace Librarian.Web.Services
{
    public interface ISecurityService
    {
        bool Authenticate(ApiCredential credentials, HttpResponseBase response);
    }
}