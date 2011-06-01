using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                GetAllProjects();
                return true;
            }
            catch (WebException)
            {
                return false;
            }
        }

        public IEnumerable<Project> GetAllProjects()
        {
            return RetrieveSet<Project>("projects");
        }

        IEnumerable<T> RetrieveSet<T>(string path)
        {
            var request = BuildRequest(path);
            using (var response = (HttpWebResponse)request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            {
                return ParseSet<T>(responseStream);
            }
        }

        HttpWebRequest BuildRequest(string path)
        {
            var requestUri = new Uri(baseUri, path);
            var request = (HttpWebRequest)WebRequest.CreateDefault(requestUri);
            request.Headers.Add("X-tinyPM-token", credential.ApiKey);
            return request;
        }

        static IEnumerable<T> ParseSet<T>(Stream stream)
        {
            return Enumerable.Empty<T>();
        }
    }
}