using System.Collections.Generic;
using System.Text;

namespace VP
{
    /// <summary>
    /// Provides helper methods for dealing with Unicode strings
    /// </summary>
    public static class Unicode
    {
        /// <summary>
        /// Splits a unicode string by the VP byte limit of 255, ensuring to keep all
        /// unicode characters intact
        /// </summary>
        /// <param name="incoming">String to split</param>
        /// <returns>Array of strings, each limited to 255 bytes in size</returns>
        public static string[] ChunkByByteLimit(string incoming)
        {
            var chunks = new List<string>();
            var chunk  = "";
            var chunkByteLength = 0;
            
            foreach (var nextChar in incoming)
            {
                var charByteLength = Encoding.UTF8.GetByteCount( nextChar.ToString() );

                if (chunkByteLength + charByteLength > 255)
                {
                    chunks.Add(chunk);
                    chunk = "";
                    chunkByteLength = 0;
                }

                chunk += nextChar;
                chunkByteLength += charByteLength;
            }

            // For final left-over chunk
            if ( !string.IsNullOrWhiteSpace(chunk) )
                chunks.Add(chunk);

            return chunks.ToArray();
        }
    }
}
