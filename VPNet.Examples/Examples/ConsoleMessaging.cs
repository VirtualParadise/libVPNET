using System;
using System.Threading;
using VP;

namespace VPNetExamples.Examples
{
    class ConsoleMessaging : BaseExampleBot
    {
        public override string Name { get { return "Console messaging"; } }
        Instance bot;

        public override void main()
        {
            Console.WriteLine("Creating new instance, connecting and entering");
            bot = new Instance("VP Bot")
                .Login(VPNetExamples.Username, VPNetExamples.Password)
                .Enter(VPNetExamples.World);

            bot.Avatars.Enter += Avatars_Enter;
            bot.GoTo();

            Console.WriteLine("Trying plain broadcast");
            bot.ConsoleBroadcast("Services", "Hello everybody!");
            bot.Wait(1000);
            Thread.Sleep(1000);

            Console.WriteLine("Trying styled broadcast");
            bot.ConsoleBroadcast("Services", "Hello everybody!", ChatTextEffect.Bold, new Color(128,0,0));
            bot.Wait(1000);
            Thread.Sleep(1000);

            for (var i = 0; i < 100; i++)
            {
                bot.Wait(100);
                Thread.Sleep(10);
            }

            bot.Avatars.Enter -= Avatars_Enter;
        }

        void Avatars_Enter(Instance sender, Avatar avatar)
        {
            Console.WriteLine("Trying specific message");
            bot.ConsoleMessage(avatar.Session, "Services", "Hello " + avatar.Name, ChatTextEffect.Bold, new Color(128,0,0));
        }

        public override void dispose()
        {
            bot.Dispose();
        }
    }
}
