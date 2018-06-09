using System;
using System.Globalization;

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

        /// <summary>
        /// Converts LastConversionDateTime string to DateTime
        /// string in "en-US" format, e.g. "08-15-13 16:13" 
        /// to culture independent DateTime {8/15/2013 4:13:00 PM}
        /// </summary>
        /// <param name="lastConversionDateTime"></param>
        /// <returns>Culture Independant DateTime</returns>
        public static DateTime LastConversion2DateTime(string lastConversionDateTime)
        {
            CultureInfo cultureInfo = new CultureInfo("en-US");
            return DateTime.Parse(lastConversionDateTime, cultureInfo, DateTimeStyles.NoCurrentDateDefault);
        }
    }
}
