

using System;
using System.Configuration;
using VPNetExamples.Common;
using VpNet.Core;

namespace VPNetExamples
{
    class Program
    {
        private static BaseExampleBot _bot;

        static void Main(string[] args)
        {
            try {
menu:
                Console.WriteLine("Bot Examples");

                Console.WriteLine("1. Hello World! Bot");
                Console.WriteLine("2. Greeter Bot");
                Console.Write("Please enter a numer (1-2): ");
                string read = Console.ReadLine();

                switch (read)
                {
                    case "1":
                        _bot = new HelloWorldBot();
                        break;
                    case "2":
                        _bot = new GreeterBot();
                        break;
                    default:
                        Console.WriteLine("Please enter an existing number");
                        goto menu;
                }
                Console.Write("Running Example {0}. Press Enter to exit and choose another example.");
                Console.ReadLine();
                _bot.Dispose();
                goto menu;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Have you entered your credentials in the app.config file?");
                Console.ReadLine();
            }
        }
    }
}
