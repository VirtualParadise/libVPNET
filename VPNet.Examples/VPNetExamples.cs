using System;
using System.Linq;
using System.Reflection;

namespace VPNetExamples
{
    class VPNetExamples
    {
        internal static string Username;
        internal static string Password;
        internal static string World;

        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("VPNet SDK demo roll. To run: VPNetExamples.exe \"user name\" \"password\" \"world\"");
                return;
            }

            Username = args[0];
            Password = args[1];
            World    = args[2];

            var demoQuery =
                from   t in Assembly.GetExecutingAssembly().GetTypes()
                where  t.IsSubclassOf(typeof(BaseExampleBot))
                select Activator.CreateInstance(t) as BaseExampleBot;
            var demos = demoQuery.ToArray<BaseExampleBot>();

        menu:
            // Iterate choices
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("VPNet SDK demo roll; please pick a number to execute:");
            Console.ForegroundColor = ConsoleColor.White;

            for (var i = 0; i < demos.Length; i++)
                Console.WriteLine("[ {0} ]\t{1}", i, demos[i].Name);

            // Get choice
            int option;
            while (true)
            {
                Console.Write("Choice: ");

                if (int.TryParse(Console.ReadLine(), out option) && option >= 0 && option < demos.Length)
                    break;
                else
                    Console.WriteLine("Invalid option");
            }

            // Fire!
            var demo = demos[option];
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\nRunning demo {0}, press CTRL+C to return to menu", demo.Name);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            demo.Execute();

            // Done
            goto menu;
        }
    }
}
