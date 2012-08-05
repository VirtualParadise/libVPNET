namespace VpNet.Core.EventData
{
    public class Chat
    {
        public string Username, Message;
        public int Session;

        public Chat(string username, string message, int session)
        {
            Username = username;
            Message = message;
            Session = session;
        }
    }
}
