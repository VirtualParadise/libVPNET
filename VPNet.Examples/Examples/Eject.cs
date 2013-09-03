using System;
using System.Threading;
using VP;

namespace VPNetExamples.Examples
{
    class Eject : BaseExampleBot
    {
        public override string Name { get { return "Eject"; } }
        Instance bot;

        public override void main()
        {
            Console.WriteLine("Creating new instance, connecting and entering");
            bot = new Instance(" ")
                .Login(VPNetExamples.Username, VPNetExamples.Password)
                .Enter(VPNetExamples.World);

            bot.Avatars.Enter += Avatars_Enter;
            while (true)
                bot.Wait(100);
        }

        void Avatars_Enter(Instance sender, Avatar avatar)
        {
            Console.WriteLine("Entered: {0}", avatar.Name);
        }

        public override void dispose()
        {
            bot.Dispose();
        }
    }
}
