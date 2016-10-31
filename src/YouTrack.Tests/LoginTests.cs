using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using YouTrack.Models;
using YouTrack.Models.Extensions;
using YouTrack.Web;

namespace YouTrack.Tests
{
    [TestFixture]
    public class LoginTests
    {
        private const string BaseAddress = "http://test.com/";

        [Test]
        public async Task TestFailedLogin()
        {
            var mockReponseHandler = new MockResponseHandler();

            var uri = new Uri($"{BaseAddress}{YoutrackDirectory.Authentication}");

            mockReponseHandler.AddFakeResponse(uri, new HttpResponseMessage(HttpStatusCode.Unauthorized));

            var clientFactory = new HttpClientFactory(mockReponseHandler);

            var client = clientFactory.Create(BaseAddress);


            var user = new User
            {
                Login = "testUser",
                Password = "password".ToSecureString()
            };

            var loginClient = new YouTrackClient(client, user);

            var success = await loginClient.AuthenticateAsync(user);

            Assert.IsFalse(success);
        }

        [Test]
        public async Task TestLogin()
        {
            var mockReponseHandler = new MockResponseHandler();

            mockReponseHandler.AddAuthenticationResponseHandler(BaseAddress);

            var clientFactory = new HttpClientFactory(mockReponseHandler);

            var client = clientFactory.Create(BaseAddress);


            var user = new User
            {
                Login = "testUser",
                Password = "password".ToSecureString()
            };

            var loginClient = new YouTrackClient(client, user);

            var success = await loginClient.AuthenticateAsync(user);

            Assert.IsTrue(success);
        }
    }
}