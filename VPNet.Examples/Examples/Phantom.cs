using System;
using System.Threading;
using VP;

namespace VPNetExamples.Examples
{
    class Phantom : BaseExampleBot
    {
        public override string Name { get { return "Phantom"; } }
        Instance bot;

        public override void main()
        {
            Console.WriteLine("Creating new instance, connecting and entering");
            bot = new Instance("VP Bot");

            bot.Data.WorldEntry += Data_GetWorldEntry;
            bot.Chat += bot_Chat;
            bot.Login(VPNetExamples.Username, VPNetExamples.Password);
            bot.Wait(1000);
            bot.Enter(VPNetExamples.World);
            bot.Wait(1000);

            while (true)
                bot.Wait(0);
        }

        void Data_GetWorldEntry(Instance sender, World world)
        {
           Console.WriteLine("{0}: {1}, {2} users", world.Name, world.State, world.UserCount);
        }

        void bot_Chat(Instance sender, ChatMessage chat)
        {
            Console.WriteLine("{0}: {1}", chat.Name, chat.Message);
        }

        public override void dispose()
        {
            bot.Dispose();
        }
    }
}
