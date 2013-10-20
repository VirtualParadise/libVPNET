using System.Text.RegularExpressions;

namespace VP.Examples
{
    class UrlSend : BaseExampleBot
    {
        public override string Name
        {
            get { return "Url Send"; }
        }
        
        Instance bot;
        Regex    command = new Regex("^/url( (?<url>.+))?$", RegexOptions.IgnoreCase);

        public override void Main(string user, string password, string world)
        {
            bot = new Instance()
                .Login(user, password, "libVPNET")
                .Enter(world)
                .Say("Hello! Type /url <url> to test Url sending to both targets");

            bot.Chat += onChat;

            while (!Disposing)
                bot.Pump();
        }

        void onChat(Instance sender, ChatMessage chat)
        {
            var match = command.Match(chat.Message);
            if (!match.Success)
                return;

            var url = match.Groups["url"].Value;

            bot.Avatars.SendUrl(chat.Session, url, UrlTarget.Browser);
            bot.Avatars.SendUrl(chat.Session, url, UrlTarget.Overlay);
        }

        public override void Dispose()
        {
            if (bot != null)
                bot.Dispose();
        }
    }
}
