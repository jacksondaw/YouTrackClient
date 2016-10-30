using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using YouTrack.Models;

namespace YouTrack.Web
{
    public class IssuesApi
    {
        private readonly YouTrackClient _client;

        public IssuesApi(YouTrackClient client)
        {
            _client = client;
        }

        public async Task CreateAsync(Issue issue)
        {
            var queryString = GetQueryString(issue);

            var request = $"{YoutrackDirectory.Issues}?{queryString}";
            var content = new MultipartFormDataContent();
            await _client.Put(request, content);
        }

        private string GetQueryString(Issue issue)
        {
            var retval =
                $"project={issue.Project}&summary={issue.Summary}&description={issue.Description}&attachments=&permittedGroup={issue.PermittedGroup}";

            return HttpUtility.UrlEncode(retval);
        }
    }

    public class YouTrackClient
    {
        private readonly HttpClient _httpClient;

        private readonly User _user;

        public YouTrackClient(HttpClient client, User user)
        {
            _httpClient = client;
            _user = user;
        }

        public async Task<bool> AuthenticateAsync(User user)
        {
            var authenicationUrl = YoutrackDirectory.Authentication;

            var password = user.Password.Read();

            var values = new Dictionary<string, string>
            {
                {"login", user.Login},
                {"password", password}
            };

            var content = new FormUrlEncodedContent(values);

            var result = await _httpClient.PostAsync(authenicationUrl, content);

            if (!result.IsSuccessStatusCode)
                return false;

            var body = await result.Content.ReadAsStringAsync();

            var retval = ParseAuthenticationMessage(body);

            if (retval == false)
                return false;
            return true;
        }

        public async Task<T> GetAsync<T>(string path)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, path);

            await EnsureAuthenticated(request);

            var postResult = await _httpClient.SendAsync(request);

            postResult.EnsureSuccessStatusCode();

            var data = await postResult.Content.ReadAsStringAsync();
            var serializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StringReader(data))
            {
                return (T) serializer.Deserialize(reader);
            }
        }

        public async Task<T> PostAsync<T>(string path, HttpContent content)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, path)
            {
                Content = content
            };

            await EnsureAuthenticated(request);

            var postResult = await _httpClient.SendAsync(request);

            postResult.EnsureSuccessStatusCode();

            var data = await postResult.Content.ReadAsStringAsync();
            var serializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StringReader(data))
            {
                return (T) serializer.Deserialize(reader);
            }
        }

        public async Task Put(string path, HttpContent content)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, path)
            {
                Content = content
            };

            await EnsureAuthenticated(request);

            var postResult = await _httpClient.SendAsync(request);

            postResult.EnsureSuccessStatusCode();
        }

        private async Task EnsureAuthenticated(HttpRequestMessage requestMessage)
        {
            if (IsAuthenticated(requestMessage)) return;

            await AuthenticateAsync(_user);
        }

        private bool IsAuthenticated(HttpRequestMessage requestMessage)
        {
            IEnumerable<string> cookies;

            var success = requestMessage.Headers.TryGetValues("set-cookie", out cookies);

            if (!success) return false;

            return cookies.Any(c => c.Contains(YouTrackConstants.SecurityCookie));
        }

        private bool ParseAuthenticationMessage(string xml)
        {
            var document = new XmlDocument();

            document.LoadXml(xml);

            var login = document.SelectSingleNode("login");

            if (login == null)
                return false;

            return true;
        }
    }

    public class YouTrackConstants
    {
        public const string SecurityCookie = "jetbrains.charisma.main.security.PRINCIPAL";
    }

    public class YoutrackDirectory
    {
        public const string Authentication = "rest/user/login";

        public const string Issues = "rest/issue";
    }
}