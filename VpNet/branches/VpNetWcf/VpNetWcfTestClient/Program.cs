using System;
using System.ServiceModel;
using System.Threading;

namespace VpNetWcfTestClient
{
    class Program
    {
        private static Timer _timer;

        static void Main(string[] args)
        {
            Console.Write("user: ");
            var user = Console.ReadLine();
            Console.Write("password: ");
            var password = Console.ReadLine();
            Console.Write("world: ");
            var world = Console.ReadLine();
            var rp = new InstanceProxy();
            if (rp.ConnectWcf() == true)
            {
                string tmp = string.Empty;

                    rp.Connect("universe.virtualparadise.org",57000);
                    rp.Login(user,password,"vpnetwcf");
                    rp.Enter(world);
                    rp.UpdateAvatar();
                    rp.EventObjectClick += new VpNet.Core.Instance.ObjectClickEvent(rp_EventObjectClick);
                    rp.Say("Hello World! from WCF!");

               //// _timer = new Timer(WaitCallback1, rp, 0, 1000); 
                
               //    // rp.SendMessage(tmp);
                    tmp = Console.ReadLine();

            }
            if (((ICommunicationObject)rp).State == CommunicationState.Opened)
                rp.Close();            
        }

        //private static void WaitCallback1(object state)
        //{
        //    ((InstanceProxy)state).Wait(0);
        //}

        static void rp_EventObjectClick(VpNet.Core.IInstance sender, int sessionId, int objectId)
        {
            Console.WriteLine("avatar session {0} clicked on object wih id {1}",sessionId,objectId);
        }
    }
}
