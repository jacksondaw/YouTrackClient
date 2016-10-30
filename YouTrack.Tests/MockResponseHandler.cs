using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

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
}