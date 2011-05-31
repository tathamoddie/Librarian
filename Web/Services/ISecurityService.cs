using Librarian.Web.Models;

namespace Librarian.Web.Services
{
    public interface ISecurityService
    {
        bool Authenticate(LoginCredentials credentials);
    }
}