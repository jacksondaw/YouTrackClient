using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace YouTrack.Web
{
    public class ClientFactory
    {
        private readonly HttpMessageHandler _handler;

        public ClientFactory()
        {
            var cookieContainer = new CookieContainer();

            _handler = new WebRequestHandler
            {
                UseCookies = true,
                CookieContainer = cookieContainer
            };
        }

        public ClientFactory(HttpMessageHandler messageHandler)
        {
            _handler = messageHandler;
        }

        public HttpClient Create(string baseAddress)
        {
            var url = new Uri(baseAddress);

            var retval = new HttpClient(_handler)
            {
                BaseAddress = url
            };

            retval.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            retval.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return retval;
        }
    }
}