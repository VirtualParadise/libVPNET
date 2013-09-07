using System;

namespace VP
{
    internal static class DateTimeExt
    {
        internal static DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        internal static int ToUnixTimestampUTC(this DateTime date)
        {
            return (int) DateTime.UtcNow.Subtract(UnixEpoch)
                .TotalSeconds;
        }

        internal static DateTime FromUnixTimestampUTC(int timestamp)
        {
            return UnixEpoch.AddSeconds(timestamp);
        }
    }
}
