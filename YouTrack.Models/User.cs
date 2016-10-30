using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace YouTrack.Models
{
    public class User
    {
        public string Login { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Jabber { get; set; }

        public DateTime LastAccess { get; set; }

        public SecureString Password { get; set; }
    }
}
