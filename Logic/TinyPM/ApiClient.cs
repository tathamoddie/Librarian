using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;

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
            return ExecuteRequest(
                "projects",
                _ => _
                    .Elements("project")
                    .Select(p => new Project
                    {
                        Id = int.Parse(p.Element("id").Value),
                        Name = p.Element("name").Value,
                        Code = p.Element("code").Value
                    })
                    .OrderBy(p => p.Name)
                    .ToArray());
        }

        public IEnumerable<UserStory> GetBacklog(int projectId)
        {
            var storyIds = ExecuteRequest(
                string.Format("project/{0}/userstories?status=PENDING", projectId),
                _ => _
                    .Elements("userStory")
                    .Select(p => int.Parse(p.Element("id").Value))
                    .ToArray());

            return storyIds
                .AsParallel()
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .Select(GetUserStory)
                .OrderBy(s => s.Position)
                .ToArray();
        }

        UserStory GetUserStory(int userStoryId)
        {
            return ExecuteRequest(
                string.Format("userstory/{0}", userStoryId),
                u => new UserStory
                {
                    Id = int.Parse(u.Element("id").Value),
                    Position = int.Parse(u.Element("position").Value),
                    Name = u.Element("name").Value
                });
        }

        T ExecuteRequest<T>(string path, Func<XElement, T> parseCallback)
        {
            var request = BuildRequest(path);
            using (var response = (HttpWebResponse)request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            {
                return ParseResponse(responseStream, parseCallback);
            }
        }

        HttpWebRequest BuildRequest(string path)
        {
            var requestUri = new Uri(baseUri, path);
            var request = (HttpWebRequest)WebRequest.CreateDefault(requestUri);
            request.Headers.Add("X-tinyPM-token", credential.ApiKey);
            return request;
        }

        static T ParseResponse<T>(Stream stream, Func<XElement, T> parseCallback)
        {
            var xmlData = XElement.Load(stream);
            return parseCallback(xmlData);
        }
    }
}