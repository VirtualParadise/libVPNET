using System;
using VP;

namespace MyBot
{
    /// <summary>
    /// A bot that enters a specified world with the specified login details, says "Hello
    /// World!" and waits for chat, entry and leave events.
    /// </summary>
    class Bot
    {
        static void Main(string[] args)
        {
            // Check given arguments
            if (args.Length < 3)
            {
                Console.WriteLine("Basic Virtual Paradise bot. To run: BasicBot.exe \"user name\" \"password\" \"world\"");
                return;
            }

            // Login bot
            var bot = new Instance()
                .Login(args[0], args[1], "Basic Bot")
                .Enter(args[2])
                .Say("Hello World!");

            // Register events
            bot.Avatars.Enter += (i,a)   => { Console.WriteLine("*** {0} enters", a.Name); };
            bot.Avatars.Leave += (i,n,s) => { Console.WriteLine("*** {0} leaves", n); };
            bot.Chat          += (i,c)   => { Console.WriteLine("{0} says: \t{1}", c.Name, c.Message); };

            // Pump
            while (true)
                bot.Pump();
        }
    }
}
