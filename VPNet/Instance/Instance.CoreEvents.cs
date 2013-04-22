using System;
using System.Collections.Generic;
using VP.Native;

namespace VP
{
    public partial class Instance : IDisposable
    {
        #region Native events and callbacks
        Dictionary<Events, EventDelegate>       _nativeEvents    = new Dictionary<Events, EventDelegate>();
        Dictionary<Callbacks, CallbackDelegate> _nativeCallbacks = new Dictionary<Callbacks, CallbackDelegate>();

        internal void SetNativeEvent(Events eventType, EventDelegate eventFunction)
        {
            _nativeEvents[eventType] = eventFunction;
            Functions.vp_event_set(pointer, (int)eventType, eventFunction);
        }

        internal void SetNativeCallback(Callbacks callbackType, CallbackDelegate callbackFunction)
        {
            _nativeCallbacks[callbackType] = callbackFunction;
            Functions.vp_callback_set(pointer, (int)callbackType, callbackFunction);
        }

        void setupEvents()
        {
            SetNativeEvent(Events.UniverseDisconnect, OnUniverseDisconnect);
            SetNativeEvent(Events.WorldDisconnect, OnWorldDisconnect);
            SetNativeEvent(Events.Chat, OnChat);
        }

        void disposeEvents()
        {
            UniverseDisconnect = null;
            WorldDisconnect = null;
            Chat = null;
        }
        #endregion

        public delegate void Event(Instance sender);
        public delegate void ChatEvent(Instance sender, Chat eventData);

        public event Event     UniverseDisconnect;
        public event Event     WorldDisconnect;
        public event ChatEvent Chat;

        internal void OnUniverseDisconnect(IntPtr sender)
        {
            if (UniverseDisconnect == null) return;
            UniverseDisconnect(this);
        }

        internal void OnWorldDisconnect(IntPtr sender)
        {
            if (WorldDisconnect == null) return;
            WorldDisconnect(this);
        }

        internal void OnChat(IntPtr sender)
        {
            if (Chat == null) return;

            Chat data;
            lock (this)
                data = new Chat(sender);

            Chat(this, data);
        } 
    }
}
