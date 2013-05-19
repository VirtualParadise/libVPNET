﻿using System;
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
        public delegate void ChatEvent(Instance sender, ChatMessage chat);
        public delegate void ConsoleEvent(Instance sender, ConsoleMessage console);

        public event Event        UniverseDisconnect;
        public event Event        WorldDisconnect;
        public event ChatEvent    Chat;
        public event ConsoleEvent Console;

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
            if (Chat == null && Console == null) return;

            ChatMessage chat;
            ChatType type;
            lock (this)
            {
                type = (ChatType) Functions.vp_int(pointer, IntAttributes.ChatType);

                if (type == ChatType.Normal)
                {
                    if (Chat == null)
                        return;
                    else
                        chat = new ChatMessage(sender);
                }
                else
                {
                    if (Console == null)
                        return;
                    else
                        chat = new ConsoleMessage(sender);
                }
            }

            if (type == ChatType.Normal)
                Chat(this, chat);
            else
                Console(this, (ConsoleMessage) chat);
        } 
    }
}