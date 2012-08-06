using System;
using System.ServiceModel;

namespace VpNetWcfTestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var rp = new InstanceProxy();
            if (rp.ConnectWcf() == true)
            {
                string tmp = string.Empty;
                    rp.Connect("universe.virtualparadise.org",57000);
                    rp.Login("username here","passwordhere","vpnetwcf");
                    rp.Enter("VP-Build");
                    rp.UpdateAvatar();
                    rp.Say("Hello World! from WCF!");
                    rp.Wait(0);
                   // rp.SendMessage(tmp);
                    tmp = Console.ReadLine();

            }
            if (((ICommunicationObject)rp).State == CommunicationState.Opened)
                rp.Close();            
        }
    }
}
