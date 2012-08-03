using System;
using System.Collections.Generic;
using VpNet.Core.EventData;
using VpNet.Core.Structs;
using VpNet.NativeApi;
using Attribute = VpNet.NativeApi.Attribute;

namespace VpNet.Core
{
    public class Instance : IDisposable
    {
        static bool _isInitialized;
        readonly IntPtr _instance;

        public Instance()
        {
            if (!_isInitialized)
            {
                int rc = Functions.vp_init(1);
                if (rc != 0)
                {
                    throw new VpException((ReasonCode)rc);
                }
                _isInitialized = true;
            }
            _instance = Functions.vp_create();
            SetNativeEvent(Events.Chat, OnChat);
            SetNativeEvent(Events.AvatarAdd, OnAvatarAdd);
            SetNativeEvent(Events.AvatarChange, OnAvatarChange);
            SetNativeEvent(Events.AvatarDelete, OnAvatarDelete);
            SetNativeEvent(Events.WorldList, OnWorldList);
            SetNativeEvent(Events.ObjectChange, OnObjectChange);
            SetNativeEvent(Events.Object, OnObjectCreate);
            SetNativeEvent(Events.ObjectClick, OnObjectClick);
            SetNativeEvent(Events.ObjectDelete, OnObjectDelete);
            SetNativeEvent(Events.UniverseDisconnect, OnUniverseDisconnect);
            SetNativeEvent(Events.WorldDisconnect, OnWorldDisconnect);
        }

        ~Instance()
        {
            if (_instance != IntPtr.Zero)
            {
                lock (this)
                {
                    Functions.vp_destroy(_instance);
                }
            }
        }

        #region Methods
        public void Wait(int milliseconds)
        {
            int rc = Functions.vp_wait(_instance, milliseconds);
            if (rc != 0)
            {
                throw new VpException((ReasonCode)rc);
            }
        }

        public void Connect(string host = "universe.virtualparadise.org", ushort port = 57000)
        {
            int rc;
            lock (this)
            {
                rc = Functions.vp_connect_universe(_instance, host, port);
            }
            if (rc != 0)
            {
                throw new VpException((ReasonCode)rc);
            }
        }

        public void Login(string username, string password, string botname)
        {
            int rc;
            lock (this)
            {
                rc = Functions.vp_login(_instance, username, password, botname);
            }
            if (rc != 0)
            {
                throw new VpException((ReasonCode)rc);
            }
        }

        public void Enter(string worldname)
        {
            int rc;
            lock (this)
            {
                rc = Functions.vp_enter(_instance, worldname);
            }
            if (rc != 0)
            {
                throw new VpException((ReasonCode)rc);
            }
        }

        /// <summary>
        /// Leave the current world
        /// </summary>
        public void Leave()
        {
            int rc;
            lock (this)
            {
                rc = Functions.vp_leave(_instance);
            }
            if (rc != 0)
            {
                throw new VpException((ReasonCode) rc);
            }
        }

        public void UpdateAvatar(
            float x=0.0f, float y=0.0f, float z=0.0f, 
            float yaw=0.0f, float pitch=0.0f)
        {
            int rc;
            lock (this)
            {
                Functions.vp_float_set(_instance, Attribute.MyX, x);
                Functions.vp_float_set(_instance, Attribute.MyY, y);
                Functions.vp_float_set(_instance, Attribute.MyZ, z);
                Functions.vp_float_set(_instance, Attribute.MyYaw, yaw);
                Functions.vp_float_set(_instance, Attribute.MyPitch, pitch);
                rc = Functions.vp_state_change(_instance);
            }
            if (rc != 0)
            {
                throw new VpException((ReasonCode)rc);
            }
        }

        public void ListWorlds()
        {
            int rc;
            lock (this)
            {
                rc = Functions.vp_world_list(_instance, 0);
            }
            if (rc != 0)
            {
                throw new VpException((ReasonCode)rc);
            }
        }

        public void QueryCell(int cellX, int cellZ)
        {
            int rc;
            lock (this)
            {
                rc = Functions.vp_query_cell(_instance, cellX, cellZ);
            }
            if (rc != 0)
                throw new VpException((ReasonCode) rc);
        }

        public void Say(string message)
        {
            int rc;
            lock (this)
            {
                rc = Functions.vp_say(_instance, message);
            }
            if (rc != 0)
            {
                throw new VpException((ReasonCode)rc);
            }
        }

        public void ChangeObject(VpObject vpObject)
        {
            int rc;
            lock (this)
            {
                Functions.vp_int_set(_instance, Attribute.ObjectId, vpObject.Id);
                Functions.vp_string_set(_instance, Attribute.ObjectAction, vpObject.Action);
                Functions.vp_string_set(_instance, Attribute.ObjectDescription, vpObject.Description);
                Functions.vp_string_set(_instance, Attribute.ObjectModel, vpObject.Model);
                Functions.vp_float_set(_instance, Attribute.ObjectRotationX, vpObject.Rotation.X);
                Functions.vp_float_set(_instance, Attribute.ObjectRotationY, vpObject.Rotation.Y);
                Functions.vp_float_set(_instance, Attribute.ObjectRotationZ, vpObject.Rotation.Z);
                Functions.vp_float_set(_instance, Attribute.ObjectX, vpObject.Position.X);
                Functions.vp_float_set(_instance, Attribute.ObjectY, vpObject.Position.Y);
                Functions.vp_float_set(_instance, Attribute.ObjectZ, vpObject.Position.Z);
                Functions.vp_float_set(_instance, Attribute.ObjectRotationAngle, vpObject.Angle);

                rc = Functions.vp_object_change(_instance);
            }
            if (rc != 0)
            {
                throw new VpException((ReasonCode)rc);
            }
        }

        #endregion
        #region Events
        
        private Dictionary<Events, EventDelegate> _nativeEvents = new Dictionary<Events, EventDelegate>();
        private void SetNativeEvent(Events eventType, EventDelegate eventFunction) {
            _nativeEvents[eventType] = eventFunction;
            Functions.vp_event_set(_instance, (int)eventType, eventFunction);
        }

        public delegate void Event(Instance sender);
        public delegate void ChatEvent(Instance sender, Chat eventData);
        public delegate void AvatarEvent(Instance sender, Avatar eventData);
        //public delegate void AvatarDeleteEvent(Instance sender, EventData.AvatarDelete eventData);
        public delegate void WorldListEvent(Instance sender, World eventData);

        public delegate void ObjectChangeEvent(Instance sender, int sessionId, VpObject objectData);
        public delegate void ObjectCreateEvent(Instance sender, int sessionId, VpObject objectData);
        public delegate void ObjectDeleteEvent(Instance sender, int sessionId, int objectId);
        public delegate void ObjectClickEvent(Instance sender, int sessionId, int objectId);

        public delegate void QueryCellResult(Instance sender, VpObject objectData);

        public event ChatEvent EventChat;
        public event AvatarEvent EventAvatarAdd;
        public event AvatarEvent EventAvatarChange;
        public event AvatarEvent EventAvatarDelete;
        public event ObjectChangeEvent EventObjectCreate;
        public event ObjectChangeEvent EventObjectChange;
        public event ObjectDeleteEvent EventObjectDelete;
        public event ObjectClickEvent EventObjectClick; 
       
        public event WorldListEvent EventWorldList;
        public event Event EventWorldSetting;
        public event Event EventWorldSettingsChanged;
        public event Event EventFriend;
        public event Event EventWorldDisconnect;
        public event Event EventUniverseDisconnect;
        public event Event EventUserAttributes;

        public event QueryCellResult EventQueryCellResult;

        #endregion
        #region Event handlers
        private void OnChat(IntPtr sender)
        {
            if (EventChat == null) return;
            Chat data;
            lock (this)
            {
                    data = new Chat(Functions.vp_string(_instance, Attribute.AvatarName),
                                    Functions.vp_string(_instance, Attribute.ChatMessage),
                                    Functions.vp_int(_instance, Attribute.AvatarSession));
            }
            EventChat(this, data);
        }

        private void OnAvatarAdd(IntPtr sender)
        {
            if (EventAvatarAdd == null) return;
            Avatar data;
            lock (this)
            {
                data = new Avatar(Functions.vp_string(_instance, Attribute.AvatarName),
                                  Functions.vp_int(_instance, Attribute.AvatarSession),
                                  Functions.vp_int(_instance, Attribute.AvatarType),
                                  Functions.vp_float(_instance, Attribute.AvatarX),
                                  Functions.vp_float(_instance, Attribute.AvatarY),
                                  Functions.vp_float(_instance, Attribute.AvatarZ),
                                  Functions.vp_float(_instance, Attribute.AvatarYaw),
                                  Functions.vp_float(_instance, Attribute.AvatarPitch));
            }
            EventAvatarAdd(this, data);
        }

        private void OnAvatarChange(IntPtr sender)
        {
            if (EventAvatarChange == null) return;
            Avatar data = null;
            lock (this)
            {
                data = new Avatar(Functions.vp_string(_instance, Attribute.AvatarName),
                                  Functions.vp_int(_instance, Attribute.AvatarSession),
                                  Functions.vp_int(_instance, Attribute.AvatarType),
                                  Functions.vp_float(_instance, Attribute.AvatarX),
                                  Functions.vp_float(_instance, Attribute.AvatarY),
                                  Functions.vp_float(_instance, Attribute.AvatarZ),
                                  Functions.vp_float(_instance, Attribute.AvatarYaw),
                                  Functions.vp_float(_instance, Attribute.AvatarPitch));
            }
            EventAvatarChange(this, data);
        }

        private void OnAvatarDelete(IntPtr sender)
        {
            if (EventAvatarDelete == null) return;
            Avatar data;
            lock (this)
            {
                data = new Avatar(
                    Functions.vp_string(_instance, Attribute.AvatarName),
                    Functions.vp_int(_instance, Attribute.AvatarSession),
                    0, 0, 0, 0, 0, 0);
            }
            EventAvatarDelete(this, data);
        }

        private void OnObjectClick(IntPtr sender)
        {
            if (EventObjectClick == null) return;
            int session;
            int objectId;
            lock(this)
            {
                session = Functions.vp_int(sender, Attribute.AvatarSession);
                objectId = Functions.vp_int(sender, Attribute.ObjectId);
            }
            EventObjectClick(this, session, objectId);
        }

        private void OnObjectDelete(IntPtr sender)
        {
            if (EventObjectDelete == null) return;
            int session;
            int objectId;
            lock (this)
            {
                session = Functions.vp_int(sender, Attribute.AvatarSession);
                objectId = Functions.vp_int(sender, Attribute.ObjectId);
            }
            EventObjectDelete(this,session, objectId);
        }

        private void OnObjectCreate(IntPtr sender)
        {
            if (EventObjectCreate == null && EventQueryCellResult == null) return;
            VpObject vpObject;
            int sessionId;
            lock (this)
            {
                sessionId = Functions.vp_int(sender, Attribute.AvatarSession);
                vpObject = new VpObject()

                               {
                                   Action = Functions.vp_string(sender, Attribute.ObjectAction),
                                   Description = Functions.vp_string(sender, Attribute.ObjectDescription),
                                   Id = Functions.vp_int(sender, Attribute.ObjectId),
                                   Model = Functions.vp_string(sender, Attribute.ObjectModel),
                                   Rotation = new Vector3(Functions.vp_float(sender, Attribute.ObjectRotationX), Functions.vp_float(sender, Attribute.ObjectRotationY), Functions.vp_float(sender, Attribute.ObjectRotationZ)),
                                   Time =
                                       new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(Functions.vp_int(sender,
                                                                                                     Attribute.
                                                                                                         ObjectTime)),
                                   ObjectType = Functions.vp_int(sender, Attribute.ObjectType),
                                   Owner = Functions.vp_int(sender, Attribute.ObjectUserId),
                                   Position = new Vector3(Functions.vp_float(sender, Attribute.ObjectX),Functions.vp_float(sender, Attribute.ObjectY),Functions.vp_float(sender, Attribute.ObjectZ)),
                                   Angle = Functions.vp_float(sender, Attribute.ObjectRotationAngle)
                               };
                
            }
            if (sessionId == -1 && EventQueryCellResult != null)
                EventQueryCellResult(this, vpObject);
            else
                if (EventObjectCreate != null)
                    EventObjectCreate(this, sessionId, vpObject);
        }

        private void OnObjectChange(IntPtr sender)
        {
            if (EventObjectChange == null) return; 
            VpObject vpObject;
            int sessionId;
            lock (this)
            {
                vpObject = new VpObject()
                               {
                                   Action = Functions.vp_string(sender, Attribute.ObjectAction),
                                   Description = Functions.vp_string(sender, Attribute.ObjectDescription),
                                   Id = Functions.vp_int(sender, Attribute.ObjectId),
                                   Model = Functions.vp_string(sender, Attribute.ObjectModel),
                                   Rotation = new Vector3(Functions.vp_float(sender, Attribute.ObjectRotationX), Functions.vp_float(sender, Attribute.ObjectRotationY), Functions.vp_float(sender, Attribute.ObjectRotationZ)),
                                   Time =
                                       new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(
                                           Functions.vp_int(sender, Attribute.ObjectTime)),
                                   ObjectType = Functions.vp_int(sender, Attribute.ObjectType),
                                   Owner = Functions.vp_int(sender, Attribute.ObjectUserId),
                                   Position = new Vector3(Functions.vp_float(sender, Attribute.ObjectX),Functions.vp_float(sender, Attribute.ObjectY),Functions.vp_float(sender, Attribute.ObjectZ)),                                 
                                   Angle = Functions.vp_float(sender, Attribute.ObjectRotationAngle),

                               };
                sessionId = Functions.vp_int(sender, Attribute.AvatarSession);
            }
            EventObjectChange(this, sessionId, vpObject);
        }

        private void OnWorldList(IntPtr sender)
        {
            if (EventWorldList == null)
                return;

            string worldName;
            World data;
            lock (this)
            {
                worldName = Functions.vp_string(_instance, Attribute.WorldName);
                data = new World
                               {
                                   Name = worldName,
                                   State = (World.WorldState) Functions.vp_int(_instance, Attribute.WorldState),
                                   UserCount = Functions.vp_int(_instance, Attribute.WorldUsers)
                               };
            }
            EventWorldList(this, data);
        }

        private void OnUniverseDisconnect(IntPtr sender)
        {
            if (EventUniverseDisconnect == null) return;
            EventUniverseDisconnect(this);
        }

        private void OnWorldDisconnect(IntPtr sender)
        {
            if (EventWorldDisconnect == null) return;
            EventWorldDisconnect(this);
        }

        #endregion

        public void ReleaseEvents()
        {
            lock (this)
            {
                EventChat = null;
                EventAvatarAdd = null;
                EventAvatarChange = null;
                EventAvatarDelete = null;
                EventObjectCreate = null;
                EventObjectChange = null;
                EventObjectDelete = null;
                EventObjectClick = null;
                EventWorldList = null;
                EventWorldSetting = null;
                EventWorldSettingsChanged = null;
                EventFriend = null;
                EventWorldDisconnect = null;
                EventUniverseDisconnect = null;
                EventUserAttributes = null;
                EventQueryCellResult = null;
            }
        }

        public void Dispose()
        {
            if (_instance != IntPtr.Zero)
            {
                Functions.vp_destroy(_instance);
            }
            GC.SuppressFinalize(this);
        }
    }
}
