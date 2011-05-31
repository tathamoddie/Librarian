using Librarian.Web.Models;

namespace Librarian.Web.Services
{
    public class SecurityService : ISecurityService
    {
        public bool Authenticate(LoginCredentials credentials)
        {
            return false;
        }
    }
}