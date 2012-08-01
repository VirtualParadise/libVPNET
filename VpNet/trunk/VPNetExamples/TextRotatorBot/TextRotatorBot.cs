using System.IO;
using VPNetExamples.Common;
using System.Threading;
using VpNet.Core;
using VpNet.Core.EventData;

namespace VPNetExamples.TextRotatorBot
{
    internal class TextRotatorBot : BaseExampleBot
    {
        private ActiveConfig<TextRotatorConfig> _config;
        private int _rotationIndex = 0;
        private bool _isRotationStarted;
        private VpObject _billBoard;

        public override void Initialize()
        {
            _config = new ActiveConfig<TextRotatorConfig>(new FileInfo(@".\TextRotatorBot\TextRotatorBotData.xml"));
            _config.OnConfigChanged += new ActiveConfig<TextRotatorConfig>.ConfigChanged(_config_OnConfigChanged);
            Instance.EventObjectChange += EventObjectChange;
        }

        void SetSign(TextRotatorConfigItem item)
        {
            _billBoard.Description = item.Text;
            Instance.ChangeObject(_billBoard);    
        }

        void EventObjectChange(Instance sender, VpObject objectData)
        {
            if (objectData.Id == _config.Config.ObjectId)
            {
                _billBoard = objectData;
                if (!_isRotationStarted)
                {
                    _isRotationStarted = true;
                    SetSign(_config.Config.TextItems[0]);
                    new Timer(RotationCallback, null, _config.Config.TextItems[0].Delay, 0);
                }
            }
        }

        private void RotationCallback(object state)
        {
            _rotationIndex++;
            if (_rotationIndex >= _config.Config.TextItems.Count)
                _rotationIndex = 0;
            SetSign(_config.Config.TextItems[_rotationIndex]);
            new Timer(RotationCallback, null, _config.Config.TextItems[_rotationIndex].Delay, 0);
        }

        void _config_OnConfigChanged(object sender, System.EventArgs e)
        {
            _rotationIndex = -1;
        }
    }
}
