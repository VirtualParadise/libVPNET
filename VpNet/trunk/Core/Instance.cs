using System;
using VpNet.Core.EventData;
using VpNet.NativeApi;
using Attribute = VpNet.NativeApi.Attribute;

namespace VpNet.Core
{
    public class Instance
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
            Functions.vp_event_set(_instance, (int)Events.Chat, OnChat);
            Functions.vp_event_set(_instance, (int)Events.AvatarAdd, OnAvatarAdd);
            Functions.vp_event_set(_instance, (int)Events.AvatarChange, OnAvatarChange);
            Functions.vp_event_set(_instance, (int)Events.AvatarDelete, OnAvatarDelete);
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

        public void Connect(string host = "virtualparadise.gotdns.com", int port = 57000)
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

        public void StateChange()
        {
            int rc = Functions.vp_state_change(_instance);
            if (rc != 0)
            {
                throw new VpException((ReasonCode)rc);
            }
        }
        #endregion
        #region Events
        public delegate void Event(Instance sender);
        public delegate void ChatEvent(Instance sender, Chat eventData);
        public delegate void AvatarEvent(Instance sender, Avatar eventData);
        //public delegate void AvatarDeleteEvent(Instance sender, EventData.AvatarDelete eventData);

        public event ChatEvent EventChat;
        public event AvatarEvent EventAvatarAdd;
        public event AvatarEvent EventAvatarChange;
        public event Event EventAvatarDelete;

        public void InvokeEventAvatarDelete()
        {
            Event handler = EventAvatarDelete;
            if (handler != null) handler(this);
        }

        public event Event EventObject;

        public void InvokeEventObject()
        {
            Event handler = EventObject;
            if (handler != null) handler(this);
        }

        public event Event EventObjectChange;

        public void InvokeEventObjectChange()
        {
            Event handler = EventObjectChange;
            if (handler != null) handler(this);
        }

        public event Event EventObjectDelete;

        public void InvokeEventObjectDelete()
        {
            Event handler = EventObjectDelete;
            if (handler != null) handler(this);
        }

        public event Event EventObjectClick;

        public void InvokeEventObjectClick()
        {
            Event handler = EventObjectClick;
            if (handler != null) handler(this);
        }

        public event Event EventWorldList;

        public void InvokeEventWorldList()
        {
            Event handler = EventWorldList;
            if (handler != null) handler(this);
        }

        public event Event EventWorldSetting;

        public void InvokeEventWorldSetting()
        {
            Event handler = EventWorldSetting;
            if (handler != null) handler(this);
        }

        public event Event EventWorldSettingsChanged;

        public void InvokeEventWorldSettingsChanged()
        {
            Event handler = EventWorldSettingsChanged;
            if (handler != null) handler(this);
        }

        public event Event EventFriend;

        public void InvokeEventFriend()
        {
            Event handler = EventFriend;
            if (handler != null) handler(this);
        }

        public event Event EventWorldDisconnect;

        public void InvokeEventWorldDisconnect()
        {
            Event handler = EventWorldDisconnect;
            if (handler != null) handler(this);
        }

        public event Event EventUniverseDisconnect;

        public void InvokeEventUniverseDisconnect()
        {
            Event handler = EventUniverseDisconnect;
            if (handler != null) handler(this);
        }

        public event Event EventUserAttributes;

        public void InvokeEventUserAttributes()
        {
            Event handler = EventUserAttributes;
            if (handler != null) handler(this);
        }

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

        private static void OnAvatarDelete(IntPtr sender)
        {
        }

        #endregion
    }
}
