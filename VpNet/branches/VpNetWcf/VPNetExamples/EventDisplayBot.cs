using System;
using VPNetExamples.Common;
using VpNet;
using VpNet.Core;
using VpNet.Core.EventData;
using VpNet.Core.Structs;

namespace VPNetExamples
{
    internal class EventDisplayBot : BaseExampleBot
    {
        public EventDisplayBot()
        {
            
        }

        public EventDisplayBot(Instance instance): base(instance){}

        public override void Initialize()
        {
            Instance.EventAvatarAdd += Instance_EventAvatarAdd;
            Instance.EventAvatarDelete += EventAvatarDelete;
            Instance.EventChat += EventChat;
            Instance.EventFriend += EventFriend;
            Instance.EventObjectClick += EventObjectClick;
            Instance.EventObjectDelete += EventObjectDelete;
            Instance.EventObjectChange += EventObjectChange;
            Instance.EventObjectCreate += EventObjectCreate;
            Instance.EventWorldList += EventWorldList;
            Instance.EventUniverseDisconnect += EventUniverseDisconnect;
            Instance.EventWorldDisconnect += EventWorldDisconnect;
        }

        void Instance_EventAvatarAdd(Instance sender, Avatar eventData)
        {
            Console.WriteLine("{0} enters world.", eventData.Name);
        }

        void EventObjectCreate(Instance sender, int sessionId, VpObject objectData)
        {
            Console.WriteLine("Created Object {0}", objectData.Id);
        }

        void EventObjectChange(Instance sender, int sessionId, VpObject objectData)
        {
            Console.WriteLine("Changed Object {0}", objectData.Id);
        }

        void EventWorldList(Instance sender, World eventData)
        {
            Console.WriteLine("World List {0}, {1} users.",eventData.Name,eventData.UserCount);
        }

        void EventObjectDelete(Instance sender, int sessionId, int objectId)
        {
            Console.WriteLine("Delete Object. {0}",objectId);
        }

        void EventObjectClick(Instance sender, int sessionId, int objectId)
        {
            Console.WriteLine("Avatar with session ID {0} clicked on object {1}.",sessionId, objectId);
        }

        void EventFriend(Instance sender)
        {
            // currently not supported.
            Console.WriteLine("Friend event.");
        }

        void EventChat(Instance sender, Chat eventData)
        {
            Console.WriteLine("{0} says {1}",eventData.Username,eventData.Message);
        }

        void EventAvatarDelete(Instance sender, Avatar eventData)
        {
            Console.WriteLine("{0} left world.", eventData.Name);
        }


        void EventAvatarAdd(Instance sender, Avatar eventData)
        {
            Console.WriteLine("{0} entered world.",eventData.Name);
        }

        void EventUniverseDisconnect(Instance sender)
        {
            Console.WriteLine("Universe disconnected.");
        }
        
        void EventWorldDisconnect(Instance sender)
        {
            Console.WriteLine("World disconnected.");
        }

        public override void Disconnect()
        {
        }
    }
}
