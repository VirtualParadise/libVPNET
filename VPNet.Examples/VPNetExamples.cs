using System;
using System.Linq;
using System.Reflection;

namespace VP.Examples
{
    class Examples
    {
        internal static string Username;
        internal static string Password;
        internal static string World;

        static BaseExampleBot currentDemo;

        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("libVPNET example roll. To run: VPNetExamples.exe \"user name\" \"password\" \"world\"");
                return;
            }

            Console.CancelKeyPress += onCancel;
            Console.Title = "libVPNET Examples";

            Username = args[0];
            Password = args[1];
            World    = args[2];

            var demoQuery =
                from   t in Assembly.GetExecutingAssembly().GetTypes()
                where  t.IsSubclassOf( typeof(BaseExampleBot) )
                select t;
            var demos = demoQuery.ToArray();

        menu:
            // Iterate choices
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Please pick a example to execute:");
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
            currentDemo = Activator.CreateInstance(demos[option]) as BaseExampleBot;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\nRunning demo {0}, press CTRL+C to return to menu", currentDemo.Name);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            currentDemo.Execute();

            // Done
            currentDemo = null;
            goto menu;
        }

        static void onCancel(object sender, ConsoleCancelEventArgs e)
        {
            if (currentDemo == null)
                return;

            currentDemo.Disposing = true;
            e.Cancel = true;
        }
    }
}
