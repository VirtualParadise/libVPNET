using System;
using System.Threading;
using VP;

namespace VPNetExamples.Examples
{
    class ConsoleMessaging : BaseExampleBot
    {
        public override string Name { get { return "Console messaging"; } }
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

            bot.Avatars.Enter += Avatars_Enter;
            bot2.Console += bot2_Console;
            bot.GoTo();
            bot2.GoTo();

            Console.WriteLine("Trying plain broadcast");
            bot.ConsoleBroadcast("hello", "Hello everybody!");
            bot.Wait(1000);
            bot2.Wait(1000);
            Thread.Sleep(1000);

            Console.WriteLine("Trying styled broadcast");
            bot.ConsoleBroadcast(ChatEffect.Bold, new Color(128,0,0), "", "How are you?");
            bot.Wait(1000);
            bot2.Wait(1000);
            Thread.Sleep(1000);

            for (var i = 0; i < 100; i++)
            {
                bot.Wait(10);
                bot2.Wait(10);
                Thread.Sleep(100);
            }

            bot.Avatars.Enter -= Avatars_Enter;
            bot2.Console -= bot2_Console;

        }

        void bot2_Console(Instance sender, ConsoleMessage console)
        {
            Console.WriteLine("Message: {0}, Color: {1}, Style: {2}, Name: {3}, Session: {4}", console.Message, console.Color, console.Effect, console.Name, console.Session);
        }

        void Avatars_Enter(Instance sender, Avatar avatar)
        {
            Console.WriteLine("Trying specific message");
            bot.ConsoleMessage(avatar.Session, ChatEffect.Italic, new Color(128,0,0), "test", "Hello " + avatar.Name);
        }

        public override void dispose()
        {
            bot.Dispose();
            bot2.Dispose();
        }
    }
}
