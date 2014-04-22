using System;

namespace VP.Extensions
{
    internal static class DateTimeExt
    {
        internal static DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        internal static DateTime FromUnixTimestampUTC(int timestamp)
        {
            return UnixEpoch.AddSeconds(timestamp);
        }
    }

    /// <summary>
    /// Provides libVPNET extensions to string literals and objects
    /// </summary>
    public static class StringExt
    {
        /// <summary>
        /// Surrounds a given string with square brackets after trimming any existing
        /// ones. This is useful for handling bot names returned by events such as
        /// <see cref="VP.AvatarsContainer.Enter"/>.
        /// </summary>
        /// <param name="str">String to format as a bot name</param>
        /// <returns>
        /// Given string formatted with a surrounding pair of square brackets
        /// </returns>
        public static string AsBotName(this string str)
        {
            return string.Format( "[{0}]", str.Trim('[', ']') );
        }
    }
}
