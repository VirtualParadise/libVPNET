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

        public KeywordBot(Instance instance) : base(instance) {}

        public KeywordBot()
        {
          
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

        void LoadConfig(FileInfo fi)
        {
            _keywordItems = SerializationHelpers.Deserialize<List<KeywordItem>>(fi);
        }

        public override void Initialize()
        {
            var config = new FileInfo(@".\KeywordBot\KeywordBotData.xml");
            
            var watcher = new FileSystemWatcher(config.Directory.FullName,config.Name);
            LoadConfig(config);
            watcher.Changed += new FileSystemEventHandler(watcher_Changed);
            watcher.EnableRaisingEvents = true;
            Instance.EventChat += EventChat;
        }

        void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            LoadConfig(new FileInfo(e.FullPath));
        }
    }
}
