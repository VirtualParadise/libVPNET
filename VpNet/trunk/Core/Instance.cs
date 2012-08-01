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

        /// <summary>
        /// Leave the current world
        /// </summary>
        public void Leave()
        {
            int rc = Functions.vp_leave(_instance);
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

        public void QueryCell(int cellX, int cellZ)
        {
            int rc = Functions.vp_query_cell(_instance, cellX, cellZ);
            if (rc != 0)
                throw new VpException((ReasonCode)rc);
        }

        public void Say(string message)
        {
            int rc = Functions.vp_say(_instance, message);
            if (rc != 0)
            {
                throw new VpException((ReasonCode)rc);
            }
        }

        public void ChangeObject(VpObject vpObject)
        {
            Functions.vp_int_set(_instance, Attribute.ObjectId, vpObject.Id);
            Functions.vp_string_set(_instance, Attribute.ObjectAction, vpObject.Action);
            Functions.vp_string_set(_instance, Attribute.ObjectDescription, vpObject.Description);
            Functions.vp_string_set(_instance,Attribute.ObjectModel, vpObject.Model);
            Functions.vp_float_set(_instance, Attribute.ObjectRotationX,  vpObject.RotationX);
            Functions.vp_float_set(_instance, Attribute.ObjectRotationY, vpObject.RotationY);
            Functions.vp_float_set(_instance, Attribute.ObjectRotationZ, vpObject.RotationZ);
            Functions.vp_float_set(_instance, Attribute.ObjectX, vpObject.X);
            Functions.vp_float_set(_instance, Attribute.ObjectY, vpObject.Y);
            Functions.vp_float_set(_instance, Attribute.ObjectZ, vpObject.Z);

            int rc = Functions.vp_object_change(_instance);
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

        public delegate void ObjectChangeEvent(Instance sender, VpObject objectData);
        public delegate void ObjectCreateEvent(Instance sender, VpObject objectData);
        public delegate void ObjectDeleteEvent(Instance sender, int id);
        public delegate void ObjectClickEvent(Instance sender, int sessionID, int objectId);


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

        private void OnObjectClick(IntPtr sender)
        {
            if (EventObjectClick != null)
                EventObjectClick(this,
                    Functions.vp_int(sender, Attribute.AvatarSession),
                    Functions.vp_int(sender, Attribute.ObjectId));
        }

        private void OnObjectDelete(IntPtr sender)
        {
            if (EventObjectDelete !=null)
                EventObjectDelete(this, Functions.vp_int(sender, Attribute.ObjectId));
        }

        private void OnObjectCreate(IntPtr sender)
        {
            if (EventObjectCreate != null)
            {
                var vpObject = new VpObject()

                                   {
                                       Action = Functions.vp_string(sender, Attribute.ObjectAction),
                                       Description = Functions.vp_string(sender, Attribute.ObjectDescription),
                                       Id = Functions.vp_int(sender, Attribute.ObjectId),
                                       Model = Functions.vp_string(sender, Attribute.ObjectModel),
                                       RotationX = Functions.vp_float(sender, Attribute.ObjectRotationX),
                                       RotationY = Functions.vp_float(sender, Attribute.ObjectRotationY),
                                       RotationZ = Functions.vp_float(sender, Attribute.ObjectRotationZ),
                                       Time = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(Functions.vp_int(sender, Attribute.ObjectTime)),
                                       ObjectType = Functions.vp_int(sender, Attribute.ObjectType),
                                       Owner = Functions.vp_int(sender, Attribute.ObjectUserId),
                                       X = Functions.vp_float(sender, Attribute.ObjectX),
                                       Y = Functions.vp_float(sender, Attribute.ObjectY),
                                       Z = Functions.vp_float(sender, Attribute.ObjectZ)
                                   };

                EventObjectCreate(this, vpObject);
            }
        }


        private void OnObjectChange(IntPtr sender)
        {
            var vpObject = new VpObject()
                               {
                                   Action =  Functions.vp_string(sender, Attribute.ObjectAction),
                                   Description =  Functions.vp_string(sender, Attribute.ObjectDescription),
                                   Id =  Functions.vp_int(sender, Attribute.ObjectId),
                                   Model =  Functions.vp_string(sender, Attribute.ObjectModel),
                                   RotationX =  Functions.vp_float(sender, Attribute.ObjectRotationX),
                                   RotationY =  Functions.vp_float(sender, Attribute.ObjectRotationY),
                                   RotationZ =  Functions.vp_float(sender, Attribute.ObjectRotationZ),
                                   Time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Functions.vp_int(sender, Attribute.ObjectTime)),/* TODO: should be a long, returns string VB_Build? */
                                   ObjectType =  Functions.vp_int(sender, Attribute.ObjectType),
                                   Owner =  Functions.vp_int(sender, Attribute.ObjectUserId),
                                   X =  Functions.vp_float(sender, Attribute.ObjectX),
                                   Y =  Functions.vp_float(sender, Attribute.ObjectY),
                                   Z =  Functions.vp_float(sender, Attribute.ObjectZ),
                                   

                               };

            EventObjectChange(this, vpObject);
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

        private void OnUniverseDisconnect(IntPtr sender)
        {
            if (EventUniverseDisconnect != null)
            {
                EventUniverseDisconnect(this);
            }
        }

        private void OnWorldDisconnect(IntPtr sender)
        {
            if (EventWorldDisconnect != null)
            {
                EventWorldDisconnect(this);
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
