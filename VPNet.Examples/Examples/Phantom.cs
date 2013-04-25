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
            bot = new Instance("VP Bot")
                .Login(VPNetExamples.Username, VPNetExamples.Password)
                .Enter(VPNetExamples.World);

            bot.Say("!bounce");
            bot.Wait(0);
        }
        public override void dispose()
        {
            bot.Dispose();
        }
    }
}
