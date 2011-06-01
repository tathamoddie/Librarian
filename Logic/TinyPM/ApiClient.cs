namespace Librarian.Logic.TinyPM
{
    public class ApiClient : IApiClient
    {
        readonly ApiCredential credentials;

        public ApiClient(IApiCredentialProvider credentialProvider)
        {
            credentials = credentialProvider.Credentials;
        }
    }
}