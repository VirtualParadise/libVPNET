using System;
using System.ServiceModel;
using VpNet.Core;

namespace VpNetWcfTestClient
{
    internal class InstanceProxy : IInstanceEvents,IInstance
    {
        private IInstance _pipeProxy;

        public bool ConnectWcf()
        {
            /*note the "DuplexChannelFactory".  This is necessary for Callbacks.
             A regular "ChannelFactory" won't work with callbacks.*/
            var pipeFactory =
                new DuplexChannelFactory<IInstance>(
                    new InstanceContext(this),
                    new NetTcpBinding(),
                    new EndpointAddress("net.tcp://127.0.0.1:8000/IInstance"));
            try
            {
                //Open the channel to the server
                _pipeProxy = pipeFactory.CreateChannel();
                //Now tell the server who is connecting
                _pipeProxy.Connect();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public void Close()
        {
            _pipeProxy.Leave();
        }

        //This function sends a string to the server so that it can broadcast
        // it to all other clients that have called Subscribe().
        public string SendMessage(string message)
        {
            try
            {
                //_pipeProxy.(message);
                return "sent >>>>  " + message;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        //This is the function that the SERVER will call
        public void OnMessageAdded(string message, DateTime timestamp)
        {
            Console.WriteLine(message + ": " + timestamp.ToString("hh:mm:ss"));
        }

        //We need to tell the server that we are leaving
        public void Dispose()
        {
            _pipeProxy.Leave();
        }

        #region Declarations

        public event Instance.ChatEvent EventChat;

        public event Instance.AvatarEvent EventAvatarAdd;

        public event Instance.AvatarEvent EventAvatarChange;

        public event Instance.AvatarEvent EventAvatarDelete;

        public event Instance.ObjectChangeEvent EventObjectCreate;

        public event Instance.ObjectChangeEvent EventObjectChange;

        public event Instance.ObjectDeleteEvent EventObjectDelete;

        public event Instance.ObjectClickEvent EventObjectClick;

        public event Instance.WorldListEvent EventWorldList;

        public event Instance.Event EventWorldSetting;

        public event Instance.Event EventWorldSettingsChanged;

        public event Instance.Event EventFriend;

        public event Instance.Event EventWorldDisconnect;

        public event Instance.Event EventUniverseDisconnect;

        public event Instance.Event EventUserAttributes;

        public event Instance.QueryCellResult EventQueryCellResult;

        #endregion 

        #region Callbacks to Proxied Events

        public void ChatCallback(Instance sender, VpNet.Core.EventData.Chat eventData)
        {
            if (EventChat != null)
                EventChat(this, eventData);
        }

        public void AvatarCallback(Instance sender, VpNet.Core.Structs.Avatar eventData)
        {
            if (EventAvatarAdd != null)
                EventAvatarAdd(this, eventData);
        }

        public void WorldListCallback(Instance sender, VpNet.Core.EventData.World eventData)
        {
            if (EventWorldList != null)
                EventWorldList(this, eventData);
        }

        public void ObjectChangeCallback(Instance sender, int sessionId, VpNet.Core.Structs.VpObject objectData)
        {
            if (EventObjectChange != null)
                EventObjectChange(this,sessionId, objectData);
        }

        public void ObjectCreateCallback(Instance sender, int sessionId, VpNet.Core.Structs.VpObject objectData)
        {
            if (EventObjectCreate != null)
                EventObjectCreate(this, sessionId, objectData);
        }

        public void ObjectDeleteCallback(Instance sender, int sessionId, int objectId)
        {
            if (EventObjectDelete != null)
                EventObjectDelete(this, sessionId, objectId);
        }

        public void ObjectClickCallback(Instance sender, int sessionId, int objectId)
        {
            if (EventObjectClick != null)
            {
                EventObjectClick(this, sessionId, objectId);
            }
        }

        public void QueryCellResultCallback(Instance sender, VpNet.Core.Structs.VpObject objectData)
        {
            if (EventQueryCellResult != null)
            {
                EventQueryCellResult(this, objectData);
            }
        }

        #endregion

        #region Proxied operations

        public void Wait(int milliseconds)
        {
            _pipeProxy.Wait(milliseconds);
        }

        public void Connect(string host = "universe.virtualparadise.org", ushort port = 57000)
        {
            _pipeProxy.Connect(host,port);
        }

        public void Login(string username, string password, string botname)
        {
            _pipeProxy.Login(username,password,botname);
        }

        public void Enter(string worldname)
        {
            _pipeProxy.Enter(worldname);
        }

        public void Leave()
        {
            _pipeProxy.Leave();
        }

        public void UpdateAvatar(float x = 0.0f, float y = 0.0f, float z = 0.0f, float yaw = 0.0f, float pitch = 0.0f)
        {
            _pipeProxy.UpdateAvatar(x,y,z,yaw,pitch);
        }

        public void ListWorlds()
        {
            _pipeProxy.ListWorlds();
        }

        public void QueryCell(int cellX, int cellZ)
        {
            _pipeProxy.QueryCell(cellX,cellZ);
        }

        public void Say(string message)
        {
            _pipeProxy.Say(message);
        }

        public void ChangeObject(VpNet.Core.Structs.VpObject vpObject)
        {
            _pipeProxy.ChangeObject(vpObject);
        }

        public void ReleaseEvents()
        {
            _pipeProxy.ReleaseEvents();
        }

        #endregion
    }
}