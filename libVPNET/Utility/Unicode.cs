using System.Collections.Generic;
using System.Text;

namespace VP
{
    public static class Unicode
    {
        public static string[] ChunkByByteLimit(string incoming)
        {
            var length = incoming.Length;
            var chunks = new List<string>();
            var chunk  = "";

            for (var i = 0; i < length; i++)
            {
                chunk += incoming[i];

                if ( Encoding.UTF8.GetByteCount(chunk) >= 255 )
                {
                    chunks.Add(chunk);
                    chunk = "";
                }
            }

            if ( !string.IsNullOrWhiteSpace(chunk) )
                chunks.Add(chunk);

            return chunks.ToArray();
        }
    }
}
