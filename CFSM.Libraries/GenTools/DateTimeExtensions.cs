using System;

namespace GenTools
{
    public static class DateTimeExtensions
    {
        public static DateTime DateFromUnixEpoch(long timestamp)
        {
            var dte = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dte.AddSeconds(timestamp);
        }

        public static DateTime DateFromUnixEpoch(string timestamp)
        {
            return DateFromUnixEpoch(Int64.Parse(timestamp));
        }

        public static long DateToUnixEpoch(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalSeconds);
        }

        public static long DateToUnixEpoch(string timestamp)
        {
            return DateToUnixEpoch(DateTime.Parse(timestamp));
        }
    }
}
