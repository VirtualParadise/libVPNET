using VPNetExamples.Common;
using VpNet.Core;
using VpNet.Core.EventData;

namespace VPNetExamples
{
    internal class GreeterBot : BaseExampleBot
    {
        public GreeterBot()
        {
            Instance.EventAvatarAdd += EventAvatarAdd;
            Instance.UpdateAvatar();
        }

        void EventAvatarAdd(Instance sender, Avatar eventData)
        {
            sender.Say(string.Format("Hello {0}.",eventData.Name));
        }

    }
}
