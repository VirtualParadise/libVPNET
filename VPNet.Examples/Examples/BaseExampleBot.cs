using System;
using VP;

namespace VPNetExamples
{
    internal abstract class BaseExampleBot
    {
        public abstract string Name { get; }

        /// <summary>
        /// The custom-defined execution code of this example; it should initialize a
        /// bot instance each time and use a while loop to pump events
        /// </summary>
        public abstract void main();

        /// <summary>
        /// Allows the example to dispose of its bot instance and do any cleanup
        /// </summary>
        public abstract void dispose();

        /// <summary>
        /// Hooks the console's cancel key press and then executes the example's defined
        /// main() method, automatically ending the bot if/when the function returns
        /// </summary>
        public void Execute()
        {
            Console.CancelKeyPress += onCancel;
            try
            {
                main();
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

            dispose();
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
