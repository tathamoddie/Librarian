using System;
using System.IO;
using System.Net;

namespace Librarian.Logic.TinyPM
{
    public class ApiClient : IApiClient
    {
        readonly ApiCredential credential;
        readonly Uri baseUri;

        public ApiClient(IApiCredentialProvider credentialProvider)
            : this(credentialProvider.Credentials)
        {}

        public ApiClient(ApiCredential credential)
        {
            this.credential = credential;

            var baseUriBuilder = new UriBuilder(this.credential.InstanceUri);
            baseUriBuilder.Path = Path.Combine(baseUriBuilder.Path, "api/");
            baseUri = baseUriBuilder.Uri;
        }

        public bool Ping()
        {
            try
            {
                ExecuteRequest("projects");
                return true;
            }
            catch (WebException)
            {
                return false;
            }
        }

        void ExecuteRequest(string path)
        {
            var requestUri = new Uri(baseUri, path);
            var request = (HttpWebRequest)WebRequest.CreateDefault(requestUri);

            request.Headers.Add("X-tinyPM-token", credential.ApiKey);

            request.GetResponse();
        }
    }
}