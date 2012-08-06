using VPNetExamples.Common;
using VpNet.Core;
using VpNet.Core.Structs;

namespace VPNetExamples
{
    internal class GreeterBot : BaseExampleBot
    {
        public GreeterBot(IInstance instance) : base(instance) { }

        public GreeterBot(){}

        void EventAvatarDelete(IInstance sender, Avatar eventData)
        {
            sender.Say(string.Format("{0} has left.", eventData.Name));
        }

        void EventAvatarAdd(IInstance sender, Avatar eventData)
        {
            sender.Say(string.Format("Hello {0}.",eventData.Name));
        }

        public override void Initialize()
        {
            Instance.EventAvatarAdd += EventAvatarAdd;
            Instance.EventAvatarDelete += EventAvatarDelete;
        }

        public override void Disconnect()
        {
        }
    }
}
