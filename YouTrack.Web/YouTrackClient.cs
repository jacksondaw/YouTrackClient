using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using YouTrack.Models;

namespace YouTrack.Web
{
    public class YouTrackClient
    {
        private readonly HttpClient _httpClient;

        public YouTrackClient(HttpClient client)
        {
            _httpClient = client;
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

    public class YoutrackDirectory
    {
        public const string Authentication = "rest/user/login";
    }
}