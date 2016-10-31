using System;

namespace YouTrack.Models.Extensions
{
    public static class DateTimExtensions
    {
        public static DateTime ConvertToDateTime(this long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(unixTime);
        }
    }
}