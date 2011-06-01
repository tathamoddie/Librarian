using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Xml;
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

            if (credential == null) return;

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

        public void SetUserStoryColor(int storyId, string color)
        {
            ExecutePut(
                string.Format("userstory/{0}", storyId),
                x =>
                {
                    x.Element("color").Value = color.ToUpperInvariant();
                });
        }

        UserStory GetUserStory(int userStoryId)
        {
            return ExecuteRequest(
                string.Format("userstory/{0}", userStoryId),
                u => new UserStory
                {
                    Id = int.Parse(u.Element("id").Value),
                    Position = int.Parse(u.Element("position").Value),
                    Name = u.Element("name").Value,
                    Description = u.Element("description").Value,
                    EstimatedEffort = u.Element("estimatedEffort") == null ? null : (double?)double.Parse(u.Element("estimatedEffort").Value),
                    ColorKey = u.Element("color").Value
                });
        }

        T ExecuteRequest<T>(string path, Func<XElement, T> parseCallback)
        {
            var request = BuildRequest(path);
            using (var response = (HttpWebResponse)request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            {
                var xmlData = XElement.Load(responseStream);
                return parseCallback(xmlData);
            }
        }

        void ExecutePut(string path, Action<XElement> modifierCallback)
        {
            XElement templateData;
            var templateRequest = BuildRequest(path);
            using (var templateResponse = (HttpWebResponse)templateRequest.GetResponse())
            using (var templateResponseStream = templateResponse.GetResponseStream())
            {
                templateData = XElement.Load(templateResponseStream);
            }

            modifierCallback(templateData);

            string updateString;
            using (var updateStringWriter = new StringWriter())
            using (var updateXmlWriter = new XmlTextWriter(updateStringWriter))
            {
                templateData.WriteTo(updateXmlWriter);

                updateString = updateStringWriter.ToString();
            }

            var updateRequest = BuildRequest(path);
            updateRequest.Method = "PUT";
            updateRequest.ContentType = MediaTypeNames.Text.Xml;
            updateRequest.ContentLength = updateString.Length;
            using (var updateRequestStream = updateRequest.GetRequestStream())
            using (var updateRequestStreamWriter = new StreamWriter(updateRequestStream))
            {
                updateRequestStreamWriter.Write(updateString);
            }

            using (updateRequest.GetResponse()) {}
        }

        HttpWebRequest BuildRequest(string path)
        {
            var requestUri = new Uri(baseUri, path);
            var request = (HttpWebRequest)WebRequest.CreateDefault(requestUri);
            request.Headers.Add("X-tinyPM-token", credential.ApiKey);
            return request;
        }
    }
}