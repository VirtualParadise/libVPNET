

using System;
using System.Configuration;
using VpNet.Core;

namespace VPNetExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            try {
            // connect to universe.
                var instance = new Instance();
                instance.Connect(ConfigurationManager.AppSettings["server"], ushort.Parse(ConfigurationManager.AppSettings["serverPort"]));
                instance.Login(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"], ConfigurationManager.AppSettings["botName"]);
                instance.Enter(ConfigurationManager.AppSettings["world"]);
                instance.Say("Hello World!");
                instance.Wait(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Have you entered your credentials in the app.config file?");
            }
            Console.ReadLine();
        }
    }
}
