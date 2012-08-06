using System;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using VpNet.Core;

namespace VpNetWcfHostConsole
{
    public class InstanceServer : IInstance
    {
        //private static List<IInstanceEvents> subscribers = new List<IInstanceEvents>();
        public ServiceHost ahost;

        private Instance _instance = new Instance();
    
        public bool ConnectWcf()
        {
            //_instance = new Instance();
            
            using (var ahost = new ServiceHost(typeof(InstanceServer),new Uri("net.tcp://127.0.0.1:8000")))
            {
                ahost.AddServiceEndpoint(typeof(IInstance),new NetTcpBinding(),"IInstance");                

                try
                {
                    ahost.Open();
                    Console.WriteLine("Successfully opened port 8000.");
                    Console.ReadLine();
                    ahost.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);

                }             
            }
            return true;
        }

        #region events 
    
        public event Instance.ChatEvent  EventChat;

        public event Instance.AvatarEvent  EventAvatarAdd;

        public event Instance.AvatarEvent  EventAvatarChange;

        public event Instance.AvatarEvent  EventAvatarDelete;

        public event Instance.ObjectChangeEvent  EventObjectCreate;

        public event Instance.ObjectChangeEvent  EventObjectChange;

        public event Instance.ObjectDeleteEvent  EventObjectDelete;

        public event Instance.ObjectClickEvent  EventObjectClick;

        public event Instance.WorldListEvent  EventWorldList;

        public event Instance.Event  EventWorldSetting;

        public event Instance.Event  EventWorldSettingsChanged;

        public event Instance.Event  EventFriend;

        public event Instance.Event  EventWorldDisconnect;

        public event Instance.Event  EventUniverseDisconnect;

        public event Instance.Event  EventUserAttributes;

        public event Instance.QueryCellResult  EventQueryCellResult;

        #endregion

        public void  Wait(int milliseconds)
        {
 	        _instance.Wait(milliseconds);
        }

        public void  Connect(string host, ushort port)
        {
            Console.WriteLine("connecting.");
 	        _instance.Connect(host,port);
        }

        public void Login(string username, string password, string botname)
        {
            Console.WriteLine("login.");
 	        _instance.Login(username, password, botname);
        }

        public void  Enter(string worldname)
        {
            Console.WriteLine("enter world.");
 	        _instance.Enter(worldname);
        }

        public void  Leave()
        {
            Console.WriteLine("leave.");
            _instance.Leave();
        }

        public void  UpdateAvatar(float x = 0.0f, float y = 0.0f, float z = 0.0f, float yaw = 0.0f, float pitch = 0.0f)
        {
            Console.WriteLine("update avatar.");
 	        _instance.UpdateAvatar(x,y,yaw,pitch);
        }

        public void  ListWorlds()
        {
 	        _instance.ListWorlds();
        }

        public void  QueryCell(int cellX, int cellZ)
        {
 	        _instance.QueryCell(cellX, cellZ);
        }

        public void  Say(string message)
        {
            Console.WriteLine("say");
 	        _instance.Say(message);
        }

        public void  ChangeObject(VpNet.Core.Structs.VpObject vpObject)
        {
 	        _instance.ChangeObject(vpObject);
        }

        public void  ReleaseEvents()
        {
            _instance.ReleaseEvents();
        }

        public void  Dispose()
        {
            _instance.Dispose();
        }
}
}
