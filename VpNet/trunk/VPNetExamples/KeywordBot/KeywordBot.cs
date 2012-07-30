using System.Collections.Generic;
using System.IO;
using VPNetExamples.Common;
using VpNet.Core;
using VpNet.Core.EventData;

namespace VPNetExamples.KeywordBot
{
    internal class KeywordBot : BaseExampleBot
    {
        public List<KeywordItem> _keywordItems;

        public KeywordBot()
        {
            _keywordItems = SerializationHelpers.Deserialize<List<KeywordItem>>(new FileInfo(@".\KeywordBot\KeywordBotData.xml"));
            Instance.EventChat += EventChat;
        }

        void EventChat(Instance sender, Chat eventData)
        {
            var message = eventData.Message.ToLower();
            foreach (var item in _keywordItems)
            {
                if (message.ToLower().Contains(item.Keyword))
                {
                    sender.Say(item.Response);
                }
            }
        }
    }
}
