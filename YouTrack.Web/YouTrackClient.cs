using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml;
using YouTrack.Models;

namespace YouTrack.Web
{
    public class YouTrackClient
    {
        private readonly CookieContainer _cookieContainer;
        private readonly HttpClient _httpClient;

        public YouTrackClient(string baseAddress)
        {
            var url = new Uri(baseAddress);

            _cookieContainer = new CookieContainer();

            var handler = new WebRequestHandler
            {
                UseCookies = true,
                CookieContainer = _cookieContainer
            };
            handler.ServerCertificateValidationCallback += ServerCertificateValidationCallback;
            _httpClient = new HttpClient(handler)
            {
                BaseAddress = url
            };
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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

            var cookie = _cookieContainer.GetCookieHeader(_httpClient.BaseAddress);

            return true;
        }

        protected virtual bool ServerCertificateValidationCallback(object sender, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            //TODO: add certificate authorization

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

        private void SetupAuthenicationCookie()
        {
        }
    }

    public class YoutrackDirectory
    {
        public const string Authentication = "/rest/user/login";
    }
}