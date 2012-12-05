namespace VpNetPlus.Math
{
    /// <summary>
    /// Paremeter for IParsable constructors for creating an instance from parsable text
    /// </summary>
    /// <remarks>This is used as a wrapper for the data as a constructor that excepts a single string may already exist</remarks>
    public struct ParsedData
    {
        public string Text;

        public ParsedData(string parsableText)
        {
            Text = parsableText;
        }

        public override string ToString()
        {
            return Text;
        }

    }
}