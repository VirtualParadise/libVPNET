using System;
using System.Collections.Generic;
using VpNet.Core.EventData;
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
        }

        ~Instance()
        {
            if (_instance != IntPtr.Zero)
            {
                Functions.vp_destroy(_instance);
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
            int rc = Functions.vp_connect_universe(_instance, host, port);
            if (rc != 0)
            {
                throw new VpException((ReasonCode)rc);
            }
        }

        public void Login(string username, string password, string botname)
        {
            int rc = Functions.vp_login(_instance, username, password, botname);
            if (rc != 0)
            {
                throw new VpException((ReasonCode)rc);
            }
        }

        public void Enter(string worldname)
        {
            int rc = Functions.vp_enter(_instance, worldname);
            if (rc != 0)
            {
                throw new VpException((ReasonCode)rc);
            }
        }

        public void UpdateAvatar(
            float x=0.0f, float y=0.0f, float z=0.0f, 
            float yaw=0.0f, float pitch=0.0f)
        {
            Functions.vp_float_set(_instance, Attribute.MyX, x);
            Functions.vp_float_set(_instance, Attribute.MyY, y);
            Functions.vp_float_set(_instance, Attribute.MyZ, z);
            Functions.vp_float_set(_instance, Attribute.MyYaw, yaw);
            Functions.vp_float_set(_instance, Attribute.MyPitch, pitch);
            int rc = Functions.vp_state_change(_instance);
            if (rc != 0)
            {
                throw new VpException((ReasonCode)rc);
            }
        }

        public void ListWorlds()
        {
            int rc = Functions.vp_world_list(_instance, 0);
            if (rc != 0)
            {
                throw new VpException((ReasonCode)rc);
            }
        }

        public void Say(string message)
        {
            int rc = Functions.vp_say(_instance, message);
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

        public event ChatEvent EventChat;
        public event AvatarEvent EventAvatarAdd;
        public event AvatarEvent EventAvatarChange;
        public event AvatarEvent EventAvatarDelete;
        public event Event EventObject;
        public event Event EventObjectChange;
        public event Event EventObjectDelete;
        public event Event EventObjectClick;
        public event WorldListEvent EventWorldList;
        public event Event EventWorldSetting;
        public event Event EventWorldSettingsChanged;
        public event Event EventFriend;
        public event Event EventWorldDisconnect;
        public event Event EventUniverseDisconnect;
        public event Event EventUserAttributes;
        #endregion
        #region Event handlers
        private void OnChat(IntPtr sender)
        {
            if (EventChat != null)
            {
                var data = new Chat(Functions.vp_string(_instance, Attribute.AvatarName),
                                                         Functions.vp_string(_instance, Attribute.ChatMessage),
                                                         Functions.vp_int(_instance, Attribute.AvatarSession));
                EventChat(this, data);
            }
        }

        private void OnAvatarAdd(IntPtr sender)
        {
            if (EventAvatarAdd != null)
            {
                var data = new Avatar(Functions.vp_string(_instance, Attribute.AvatarName),
                                                                     Functions.vp_int(_instance, Attribute.AvatarSession),
                                                                     Functions.vp_int(_instance, Attribute.AvatarType),
                                                                     Functions.vp_float(_instance, Attribute.AvatarX),
                                                                     Functions.vp_float(_instance, Attribute.AvatarY),
                                                                     Functions.vp_float(_instance, Attribute.AvatarZ),
                                                                     Functions.vp_float(_instance, Attribute.AvatarYaw),
                                                                     Functions.vp_float(_instance, Attribute.AvatarPitch));
                EventAvatarAdd(this, data);
            }
        }

        private void OnAvatarChange(IntPtr sender)
        {
            if (EventAvatarChange != null)
            {
                var data = new Avatar(Functions.vp_string(_instance, Attribute.AvatarName),
                                                                     Functions.vp_int(_instance, Attribute.AvatarSession),
                                                                     Functions.vp_int(_instance, Attribute.AvatarType),
                                                                     Functions.vp_float(_instance, Attribute.AvatarX),
                                                                     Functions.vp_float(_instance, Attribute.AvatarY),
                                                                     Functions.vp_float(_instance, Attribute.AvatarZ),
                                                                     Functions.vp_float(_instance, Attribute.AvatarYaw),
                                                                     Functions.vp_float(_instance, Attribute.AvatarPitch));
                EventAvatarChange(this, data);
            }
        }

        private void OnAvatarDelete(IntPtr sender)
        {
            if (EventAvatarDelete != null)
            {
                var data = new Avatar(
                    Functions.vp_string(_instance, Attribute.AvatarName),
                    Functions.vp_int(_instance, Attribute.AvatarSession),
                    0, 0, 0, 0, 0, 0);
                EventAvatarDelete(this, data);
            }
        }

        private void OnWorldList(IntPtr sender)
        {
            var worldName = Functions.vp_string(_instance, Attribute.WorldName);
            var data = new World
            {
                Name = worldName,
                State = (World.WorldState)Functions.vp_int(_instance, Attribute.WorldState),
                UserCount = Functions.vp_int(_instance, Attribute.WorldUsers)
            };

            if (EventWorldList != null)
            {
                EventWorldList(this, data);
            }

        }
        #endregion

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
