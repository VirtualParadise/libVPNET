using System;

namespace VP
{
    internal static class DateTimeExt
    {
        public static int ToUnixTimestampUTC(this DateTime date)
        {
            return (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                .TotalSeconds;
        }

        public static DateTime FromUnixTimestampUTC(int timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp);
        }
    }
}
