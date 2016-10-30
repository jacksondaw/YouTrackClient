using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using YouTrack.Models;
using YouTrack.Models.Extensions;
using YouTrack.Web;

namespace YouTrack.Tests
{
    public class MockResponseHandler : DelegatingHandler
    {
        private readonly Dictionary<Uri, HttpResponseMessage> _mockReposonses =
            new Dictionary<Uri, HttpResponseMessage>();

        public void AddFakeResponse(Uri uri, HttpResponseMessage responseMessage)
        {
            _mockReposonses.Add(uri, responseMessage);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (_mockReposonses.ContainsKey(request.RequestUri))
                return await Task.FromResult(_mockReposonses[request.RequestUri]);
            return new HttpResponseMessage(HttpStatusCode.NotFound) {RequestMessage = request};
        }
    }

    [TestFixture]
    public class LoginTests
    {
        [Test]
        public async Task TestLogin()
        {
            var mockReponseHandler = new MockResponseHandler();

            var stringContent = new StringContent("<login>ok</login>");

            var baseAddress = "http://test.com/";
            var uri = new Uri($"{baseAddress}{YoutrackDirectory.Authentication}");

            mockReponseHandler.AddFakeResponse(uri, new HttpResponseMessage(HttpStatusCode.Accepted)
            {
                Content = stringContent
            });

            var clientFactory = new ClientFactory(mockReponseHandler);


            var client = clientFactory.Create(baseAddress);


            var loginClient = new YouTrackClient(client);

            var user = new User
            {
                Login = "testUser",
                Password = "password".ToSecureString()
            };

            var success = await loginClient.AuthenticateAsync(user);

            Assert.IsTrue(success);
        }
    }
}