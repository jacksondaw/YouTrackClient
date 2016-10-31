using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NUnit.Framework;
using YouTrack.Models;
using YouTrack.Models.Extensions;
using YouTrack.Web;

namespace YouTrack.Tests
{
    internal static class TestAuthenticationExtensions
    {
        public static void AddAuthenticationCookie(this HttpResponseMessage response)
        {
            var cookie = new Cookie(YouTrackConstants.SecurityCookie,
                "NDgxMzQ5NGQxMzdlMTYzMWJiYTMwMWQ1YWNhYjZlN2JiN2FhNzRjZTExODVkNDU2NTY1ZWY1MWQ3Mzc2NzdiMjpyb290;Path=/;Expires=Thu, 12-May-2011 16:37:10 GMT");

            var cookieBuilder =
                new StringBuilder(HttpUtility.UrlEncode(cookie.Name) + "=" + HttpUtility.UrlEncode(cookie.Value));
            if (cookie.HttpOnly)
                cookieBuilder.Append("; HttpOnly");

            if (cookie.Secure)
                cookieBuilder.Append("; Secure");

            response.Headers.Add("Set-Cookie", cookieBuilder.ToString());
        }

        public static void AddAuthenticationResponseHandler(this MockResponseHandler messageHandler, string baseAddress)
        {
            var stringContent = new StringContent("<login>ok</login>");

            var uri = new Uri($"{baseAddress}{YoutrackDirectory.Authentication}");

            messageHandler.AddFakeResponse(uri, new HttpResponseMessage(HttpStatusCode.Accepted)
            {
                Content = stringContent
            });
        }
    }

    [TestFixture]
    public class IssueTests
    {
        private const string BaseAddress = "http://test.com/";


        private MockResponseHandler GetAuthenticatedMessageHandler()
        {
            var mockReponseHandler = new MockResponseHandler();

            var response = new HttpResponseMessage(HttpStatusCode.Accepted);

            response.AddAuthenticationCookie();
            mockReponseHandler.AddAuthenticationResponseHandler(BaseAddress);

            return mockReponseHandler;
        }

        [Test]
        public async Task TestCreate()
        {
            var messageHandler = GetAuthenticatedMessageHandler();

            var uri = new Uri($"{BaseAddress}{YoutrackDirectory.Issues}");

            messageHandler.AddFakeResponse(uri, new HttpResponseMessage(HttpStatusCode.Accepted));

            var clientFactory = new HttpClientFactory();

            var client = clientFactory.Create(BaseAddress);


            var user = new User
            {
                Login = "user",
                Password = "password".ToSecureString()
            };

            var youTrackClient = new YouTrackClient(client, user);


            var issuesApi = new IssuesApi(youTrackClient);

            var issue = new Issue
            {
                Project = "TestProject",
                Description = "An issues has occured",
                Summary = "Some issue"
            };

            await issuesApi.CreateAsync(issue);
        }
    }
}