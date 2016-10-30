using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using YouTrack.Models;
using YouTrack.Models.Extensions;
using YouTrack.Web;

namespace YouTrack.Tests
{
    [TestFixture]
    public class LoginTests
    {
        [Test]
        public async Task TestLogin()
        {
            var baseAddress = "http://test.com/";
            var loginClient = new YouTrackClient(baseAddress);

            var user = new User()
            {
                Login = "testUser",
                Password = "password".ToSecureString()
            };

            var success = await loginClient.AuthenticateAsync(user);

            Assert.IsTrue(success);
        }
    }
}
