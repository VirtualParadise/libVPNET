using System;
using System.Collections.Generic;
using VP;

namespace VP.Examples
{
    abstract class BaseExampleBot
    {
        public abstract string Name { get; }

        /// <summary>
        /// The custom-defined execution code of this example; it should initialize a
        /// bot instance each time and use a while loop to pump events
        /// </summary>
        public abstract void Main(string username, string password, string world);

        /// <summary>
        /// Allows the example to dispose of its bot instance and do any cleanup
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Hooks the console's cancel key press and then executes the example's defined
        /// main() method, automatically ending the bot if/when the function returns
        /// </summary>
        public void Execute()
        {
            Console.CancelKeyPress += onCancel;
            try
            {
                Main(Examples.Username, Examples.Password, Examples.World);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Demo hit exception: {0}", e.Message);
                Console.WriteLine("{0}", e.StackTrace);
            }

            End();
        }

        /// <summary>
        /// Unhooks the console's cancel key press
        /// </summary>
        public void End()
        {
            Console.CancelKeyPress -= onCancel;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Ending demo {0}...", Name);

            Dispose();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("*****\n\n");
        }

        void onCancel(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            End();
        }
    }
}
