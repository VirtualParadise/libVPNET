using System;
using VPNetExamples.Common;
using VpNet.Core;
using VpNet.Core.EventData;

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
            Instance.EventAvatarAdd += EventAvatarAdd;
            Instance.EventAvatarDelete += EventAvatarDelete;
            Instance.EventChat += EventChat;
            Instance.EventFriend += EventFriend;
            Instance.EventObject += EventObject;
            Instance.EventObjectClick += EventObjectClick;
            Instance.EventObjectDelete += EventObjectDelete;
            Instance.EventWorldList += EventWorldList;
        }

        void EventWorldList(Instance sender, World eventData)
        {
            Console.WriteLine("World List {0}, {1} users.",eventData.Name,eventData.UserCount);
        }

        void EventObjectDelete(Instance sender)
        {
            // currently not supported.
            Console.WriteLine("Delete Object.");
        }

        void EventObjectClick(Instance sender)
        {
            // currently not supported.
            Console.WriteLine("Click on object.");
        }

        void EventObject(Instance sender)
        {
            // currently not supported.
            Console.WriteLine("New Object.");
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


        void EventAvatarAdd(Instance sender, VpNet.Core.EventData.Avatar eventData)
        {
            Console.WriteLine("{0} entered world.",eventData.Name);
        }
    }
}
