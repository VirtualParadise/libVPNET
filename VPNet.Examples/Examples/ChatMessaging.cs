using System;
using System.Threading;
using VP;

namespace VPNetExamples.Examples
{
    class ChatMessaging : BaseExampleBot
    {
        public override string Name { get { return "Chat messaging"; } }
        Instance bot;
        Instance bot2;

        public override void main()
        {
            Console.WriteLine("Creating new instance, connecting and entering");
            bot = new Instance("Sender")
                .Login(VPNetExamples.Username, VPNetExamples.Password)
                .Enter(VPNetExamples.World);

            bot2 = new Instance("Receiver")
                .Login(VPNetExamples.Username, VPNetExamples.Password)
                .Enter(VPNetExamples.World);

            bot.Chat  += bot_Chat;
            bot2.Chat += bot2_Chat;
            bot.GoTo();
            bot2.GoTo();
            bot.Wait(10000);
            bot2.Wait(10000);

            Console.WriteLine("Trying chat message");
            bot.Say("Hello, world!");
            bot.Wait(10000);
            bot2.Wait(10000);
            bot.Wait(10000);
            bot2.Wait(10000);
            bot.Wait(10000);
            bot2.Wait(10000);
            bot.Wait(10000);
            bot2.Wait(10000);
            bot.Wait(10000);
            bot2.Wait(10000);

            bot.Chat  -= bot_Chat;
            bot2.Chat -= bot2_Chat;
        }

        void bot_Chat(Instance sender, ChatMessage console)
        {
            Console.WriteLine("#1 Message: {0},  Name: {1}, Session: {2}", console.Message, console.Name, console.Session);
        }

        void bot2_Chat(Instance sender, ChatMessage console)
        {
            Console.WriteLine("#2 Message: {0},  Name: {1}, Session: {2}", console.Message, console.Name, console.Session);
        }

        public override void dispose()
        {
            if (bot != null)  bot.Dispose();
            if (bot2 != null) bot2.Dispose();
        }
    }
}
