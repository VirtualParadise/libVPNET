using System;
using System.Threading;
using VP;

namespace VPNetExamples.Examples
{
    class ClickEvents : BaseExampleBot
    {
        public override string Name { get { return "Click events"; } }
        Instance bot;

        public override void main()
        {
            Console.WriteLine("Creating new instance, connecting and entering");
            bot = new Instance("VP Bot")
                .Login(VPNetExamples.Username, VPNetExamples.Password)
                .Enter(VPNetExamples.World);

            bot.Property.ObjectClick += Property_ObjectClick;
            bot.GoTo();

            while (true)
                bot.Wait(100);

            bot.Property.ObjectClick -= Property_ObjectClick;
        }

        void Property_ObjectClick(Instance sender, ObjectClick click)
        {
            Console.WriteLine("Click at {0}, {1}, {2} on {3} by {4}", click.X, click.Y, click.Z, click.Id, click.Session);
        }

        public override void dispose()
        {
            bot.Dispose();
        }
    }
}
