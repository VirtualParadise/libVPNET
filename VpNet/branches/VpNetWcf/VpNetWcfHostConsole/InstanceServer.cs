using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using VpNet.Core;
using System.Linq;
using System.Linq.Expressions;

namespace VpNetWcfHostConsole
{
    public class Client
    {
        public IInstanceEvents Callback { get; set; }
        public Instance Instance { get; set; }
    }

     [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class InstanceServer : IInstance
    {
        private ServiceHost _serviceHost;

        private Dictionary<string, Client> _clients = new Dictionary<string, Client>();

         public InstanceServer()
         {
             _timer = new Timer(TimerWaitCallback, this, 0, 100);
         }

        public bool ConnectWcf()
        {
            using (_serviceHost = new ServiceHost(typeof(InstanceServer),new Uri("net.tcp://platform3d.com:8000")))
            {
                var binding = new NetTcpBinding {Security = {Mode = SecurityMode.None}};

                _serviceHost.AddServiceEndpoint(typeof(IInstance),binding,"IInstance");                

                try
                {
                    _serviceHost.Open();
                    Console.WriteLine("Successfully opened port 8000.");
                    Console.ReadLine();
                    _serviceHost.Close();
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
            foreach (var client in _clients)
            {
                client.Value.Instance.Wait(milliseconds);
            }
        }

         private Timer _timer;


         private Client CurrentClient 
         {
             get { return _clients[OperationContext.Current.SessionId]; }
         }

        public void  Connect(string host, ushort port)
        {
            Console.WriteLine("connecting. wcf session {0}", OperationContext.Current.SessionId);
            var callback = OperationContext.Current.GetCallbackChannel<IInstanceEvents>();
            var instance = new Instance();
            _clients.Add(OperationContext.Current.SessionId, new Client() {Instance = instance, Callback = callback});
 	        instance.Connect(host,port);
        }

        public void Login(string username, string password, string botname)
        {
            Console.WriteLine("login.");
            CurrentClient.Instance.Login(username, password, botname);
        }

        public void  Enter(string worldname)
        {
            Console.WriteLine("enter world.");
            CurrentClient.Instance.Enter(worldname);
            CurrentClient.Instance.EventObjectClick += _instance_EventObjectClick;
            CurrentClient.Instance.EventObjectCreate += new Instance.ObjectChangeEvent(Instance_EventObjectCreate);
            CurrentClient.Instance.EventObjectChange += new Instance.ObjectChangeEvent(Instance_EventObjectChange);
            CurrentClient.Instance.EventObjectDelete += new Instance.ObjectDeleteEvent(Instance_EventObjectDelete);
            CurrentClient.Instance.EventAvatarAdd += new Instance.AvatarEvent(Instance_EventAvatarAdd);
            CurrentClient.Instance.EventAvatarChange += new Instance.AvatarEvent(Instance_EventAvatarChange);
            CurrentClient.Instance.EventAvatarDelete  += new Instance.AvatarEvent(Instance_EventAvatarChange);
            CurrentClient.Instance.EventChat += new Instance.ChatEvent(Instance_EventChat);
            CurrentClient.Instance.EventUniverseDisconnect += new Instance.Event(Instance_EventUniverseDisconnect);
            CurrentClient.Instance.EventWorldDisconnect += new Instance.Event(Instance_EventWorldDisconnect);
            CurrentClient.Instance.EventWorldList += new Instance.WorldListEvent(Instance_EventWorldList);
            //CurrentClient.Instance.EventWorldSetting += new Instance.Event(Instance_EventWorldSetting);
            //CurrentClient.Instance.EventWorldSettingsChanged += new Instance.Event(Instance_EventWorldSettingsChanged);

        }

        void Instance_EventWorldList(IInstance sender, VpNet.Core.EventData.World eventData)
        {
            var client = _clients.Values.FirstOrDefault(p => p.Instance == sender);
            if (client != null) client.Callback.WorldListCallback(eventData);
        }

         void Instance_EventWorldDisconnect(IInstance sender)
        {
            var client = _clients.Values.FirstOrDefault(p => p.Instance == sender);
            if (client != null) client.Callback.WorldDisconnectCallback();
        }

        void Instance_EventUniverseDisconnect(IInstance sender)
        {
            var client = _clients.Values.FirstOrDefault(p => p.Instance == sender);
            if (client != null) client.Callback.UniverseDisconnectCallback();
        }

        void Instance_EventChat(IInstance sender, VpNet.Core.EventData.Chat eventData)
        {
            var client = _clients.Values.FirstOrDefault(p => p.Instance == sender);
            if (client != null) client.Callback.ChatCallback(eventData);
        }

        void Instance_EventAvatarChange(IInstance sender, VpNet.Core.Structs.Avatar eventData)
        {
            var client = _clients.Values.FirstOrDefault(p => p.Instance == sender);
            if (client != null) client.Callback.AvatarCallback(eventData);
        }

        void Instance_EventAvatarAdd(IInstance sender, VpNet.Core.Structs.Avatar eventData)
        {
            var client = _clients.Values.FirstOrDefault(p => p.Instance == sender);
            if (client != null) client.Callback.AvatarCallback(eventData);
        }

        void Instance_EventObjectDelete(IInstance sender, int sessionId, int objectId)
        {
            var client = _clients.Values.FirstOrDefault(p => p.Instance == sender);
            if (client != null) client.Callback.ObjectDeleteCallback(sessionId, objectId);
        }

        void Instance_EventObjectChange(IInstance sender, int sessionId, VpNet.Core.Structs.VpObject objectData)
        {
            var client = _clients.Values.FirstOrDefault(p => p.Instance == sender);
            if (client != null) client.Callback.ObjectChangeCallback(sessionId, objectData);
        }

        void Instance_EventObjectCreate(IInstance sender, int sessionId, VpNet.Core.Structs.VpObject objectData)
        {
            var client = _clients.Values.FirstOrDefault(p => p.Instance == sender);
            if (client != null) client.Callback.ObjectCreateCallback(sessionId, objectData);
        }

        private void TimerWaitCallback(object state)
        {
            Wait(0);
        }

        void _instance_EventObjectClick(IInstance sender, int sessionId, int objectId)
        {
            var client = _clients.Values.FirstOrDefault(p => p.Instance == sender);
            if (client != null) client.Callback.ObjectClickCallback(sessionId, objectId);
        }

        public void  Leave()
        {
            Console.WriteLine("leave.");
            CurrentClient.Instance.Leave();
        }

        public void  UpdateAvatar(float x = 0.0f, float y = 0.0f, float z = 0.0f, float yaw = 0.0f, float pitch = 0.0f)
        {
            Console.WriteLine("update avatar.");
            CurrentClient.Instance.UpdateAvatar(x, y, yaw, pitch);


        }



        public void  ListWorlds()
        {
            CurrentClient.Instance.ListWorlds();
        }

        public void  QueryCell(int cellX, int cellZ)
        {
            CurrentClient.Instance.QueryCell(cellX, cellZ);
        }

        public void  Say(string message)
        {
            Console.WriteLine("say");
            CurrentClient.Instance.Say(message);
        }

        public void  ChangeObject(VpNet.Core.Structs.VpObject vpObject)
        {
            CurrentClient.Instance.ChangeObject(vpObject);
        }

        public void  ReleaseEvents()
        {
            CurrentClient.Instance.ReleaseEvents();
        }

        public void  Dispose()
        {
            Console.WriteLine("dispose");
        }
}
}
