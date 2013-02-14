using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VP;

namespace VPNetExamples.Examples
{
    class SimpleHelloWorld : BaseExampleBot
    {
        public override string Name { get { return "Simple 'Hello World!'"; } }
        Instance bot;

        public override void main()
        {
            Console.WriteLine("Creating new instance, connecting and entering");
            bot = new Instance("VP Bot")
                .Login(VPNetExamples.Username, VPNetExamples.Password)
                .Enter(VPNetExamples.World);

            Console.WriteLine("Saying hello");
            bot.GoTo();
            bot.Say("Hello world!");
            bot.Wait(1000);
        }

        public override void dispose()
        {
            bot.Dispose();
        }
    }
}
