using System;
using System.Threading;
using VP;

namespace VPNetExamples.Examples
{
    class AvatarEvents : BaseExampleBot
    {
        public override string Name { get { return "Avatar events"; } }
        Instance bot;

        public override void main()
        {
            Console.WriteLine("Creating new instance, connecting and entering");
            bot = new Instance("VP Bot")
                .Login(VPNetExamples.Username, VPNetExamples.Password)
                .Enter(VPNetExamples.World);

            bot.Avatars.Enter += AvEvent;
            bot.Avatars.Leave += AvEvent;
            bot.GoTo();

            while (true)
                bot.Wait(100);
        }

        void AvEvent(Instance sender, Avatar avatar)
        {
            Console.WriteLine("User #{0} {1} entered/left with session {2}", avatar.Id, avatar.Name, avatar.Session);
        }

        public override void dispose()
        {
            if (bot != null)
                bot.Dispose();
        }
    }
}
