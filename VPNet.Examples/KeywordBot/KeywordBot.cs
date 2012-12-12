using System.Collections.Generic;
using System.IO;
using VPNetExamples.Common;
using VP;
using VpNet.Core.EventData;

namespace VPNetExamples.KeywordBot
{
    internal class KeywordBot : BaseExampleBot
    {
 
        private ActiveConfig<List<KeywordItem>> _keywordItems; 

        public KeywordBot(Instance instance) : base(instance) {}

        public KeywordBot()
        {
          
        }

        void EventChat(Instance sender, Chat eventData)
        {
            if (eventData.Username.StartsWith("["))
                return;
            var message = eventData.Message.ToLower();
            foreach (var item in _keywordItems.Config)
            {
                if (message.ToLower().Contains(item.Keyword))
                {
                    sender.Say(item.Response);
                }
            }
        }

        public override void Initialize()
        {
            _keywordItems = new ActiveConfig<List<KeywordItem>>(new FileInfo(@".\KeywordBot\KeywordBotData.xml"));
            Instance.EventChat += EventChat;
        }

        public override void Disconnect()
        {
            Instance.EventChat -= EventChat;
        }
    }
}
