using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace YouTrack.Models.Extensions
{
    public static class StringExtensions
    {
        public static SecureString ToSecureString(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return null;
            SecureString Result = new SecureString();
            foreach (char c in source)
                Result.AppendChar(c);
            return Result;
        }
    }
}
