using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VP.Native;
using VP.Interfaces;

namespace VP
{
    /// <summary>
    /// Container class for Instance's communication-related members
    /// </summary>
    public class InstanceComms : IInstanceContainer
    {
        internal Instance instance;

        #region IInstanceContainer
        public void SetNativeEvents()
        {
            instance.SetNativeEvent(Events.Chat, OnChat);
        }

        public void Dispose()
        {
            Chat = null;
        } 
        #endregion

        #region Events
        public delegate void ChatEvent(Instance sender, Chat eventData);

        public event ChatEvent Chat;
        public event Instance.Event Friend;
        #endregion

        #region Methods
        /// <summary>
        /// Sends a chat message to the current world
        /// </summary>
        public void Say(string message)
        {
            int rc;
            lock (instance)
                rc = Functions.vp_say(instance.pointer, message);

            if (rc != 0) throw new VPException((ReasonCode)rc);
        }

        /// <summary>
        /// Sends a formatted chat message to current world
        /// </summary>
        public void Say(string message, params object[] parts)
        { Say(string.Format(message, parts)); } 
        #endregion

        #region Event handlers
        internal void OnChat(IntPtr sender)
        {
            if (Chat == null) return;
            Chat data;
            lock (instance)
                data = new Chat
                {
                    Name = Functions.vp_string(instance.pointer, VPAttribute.AvatarName),
                    Message = Functions.vp_string(instance.pointer, VPAttribute.ChatMessage),
                    Session = Functions.vp_int(instance.pointer, VPAttribute.AvatarSession)
                };

            Chat(instance, data);
        } 
        #endregion

        
    }
}
