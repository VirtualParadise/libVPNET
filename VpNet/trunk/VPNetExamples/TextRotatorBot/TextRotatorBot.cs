using System;
using System.IO;
using System.Linq;
using VPNetExamples.Common;
using System.Threading;
using VPNetExamples.Common.ActionInterpreter.Commands;
using VpNet.Core;
using VpNet.Core.EventData;

namespace VPNetExamples.TextRotatorBot
{
    internal class TextRotatorBot : BaseExampleBot
    {
        private ActiveConfig<TextRotatorConfig> _config;
        private int _rotationIndex;
        private VpObject _billBoard;
        private Timer _timer;
        private const string ConfigFile = @".\TextRotatorBot\TextRotatorBotData.xml";

        public TextRotatorBot(){}

        public TextRotatorBot(Instance instance) : base(instance){}

        public override void Initialize()
        {
            _config = new ActiveConfig<TextRotatorConfig>(new FileInfo(ConfigFile));
            _config.OnConfigChanged += OnConfigChanged;
            if (_billBoard != null)
                RotationCallback(null);
            Instance.EventQueryCellResult += EventQueryCellResult;
            Instance.QueryCell(_config.Config.CellX, _config.Config.CellY);
        }

        void EventQueryCellResult(Instance sender, VpObject objectData)
        {
            if (Interpreter.Interpret(objectData).SelectMany(trigger => trigger.Commands).OfType<ACName>().Any(command => command.Name == _config.Config.Name))
            {
                _billBoard = objectData;
                SetSign(_config.Config.TextItems[0]);
                _timer = new Timer(RotationCallback, null, _config.Config.TextItems[0].Delay, 0);
            }
        }

        void SetSign(TextRotatorConfigItem item)
        {
            _billBoard.Description = item.Text;
            Instance.ChangeObject(_billBoard);    
        }

        private void RotationCallback(object state)
        {
            if (_timer != null) _timer.Dispose();
            _timer = new Timer(RotationCallback, null, _config.Config.TextItems[_rotationIndex].Delay, 0);
            _rotationIndex++;
            if (_rotationIndex >= _config.Config.TextItems.Count)
                _rotationIndex = 0;
            SetSign(_config.Config.TextItems[_rotationIndex]);
            
        }

        void OnConfigChanged(object sender, EventArgs e)
        {
            _rotationIndex = -1;
        }

        public override void Disconnect()
        {
            if (_timer != null) _timer.Dispose();
        }
    }
}
