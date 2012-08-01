

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
                Console.WriteLine("0. Run all Bots (from a single vp sdk instance)");
                Console.WriteLine("1. Hello World! Bot");
                Console.WriteLine("2. Greeter Bot");
                Console.WriteLine("3. Keyword Bot");
                Console.WriteLine("4. Event Display Bot");
                Console.WriteLine("5. Text Rotator Bot"); 
                Console.Write("Please enter a numer (0-5): ");
                string read = Console.ReadLine();

                switch (read)
                {
                    case "0":
                        _bot = new GreeterBot();
                        _bot.AttachBot<HelloWorldBot>();
                        _bot.AttachBot<KeywordBot.KeywordBot>();
                        _bot.AttachBot<EventDisplayBot>();
                        _bot.AttachBot<TextRotatorBot.TextRotatorBot>();
                        break;
                    case "1":
                        _bot = new HelloWorldBot();
                        break;
                    case "2":
                        _bot = new GreeterBot();
                        break;
                    case "3":
                        _bot = new KeywordBot.KeywordBot();
                        break;
                    case "4":
                        _bot = new EventDisplayBot();
                        break;
                    case "5":
                        _bot = new TextRotatorBot.TextRotatorBot();
                        break;
                    default:
                        Console.WriteLine("Please enter an existing number");
                        goto menu;
                }
                _bot.Initialize();
                Console.WriteLine(string.Format("Running Example {0}. Press Enter to exit and choose another example.",read));
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
