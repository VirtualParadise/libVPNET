using System;
using System.IO;
using System.Linq;
using VPNetExamples.Common;
using VPNetExamples.Common.ActionInterpreter.Commands;
using VpNet.Core;
using VpNet.Core.Structs;

namespace VPNetExamples.TextRotatorBot
{
    internal class TextRotatorBot : BaseExampleBot
    {
        private ActiveConfig<TextRotatorConfig> _config;
        private int _rotationIndex;
        private VpObject _billBoard;
        private TimerT<TextRotatorBot> _timer;
        private const string ConfigFile = @".\TextRotatorBot\TextRotatorBotData.xml";

        public TextRotatorBot(){}

        public TextRotatorBot(Instance instance) : base(instance){}

        public override void Initialize()
        {
            _config = new ActiveConfig<TextRotatorConfig>(new FileInfo(ConfigFile));
            _config.OnConfigChanged += OnConfigChanged;
            Instance.EventQueryCellResult += EventQueryCellResult;
            Instance.QueryCell(_config.Config.CellX, _config.Config.CellY);
        }

        void EventQueryCellResult(Instance sender, VpObject objectData)
        {
            if (Interpreter.Interpret(objectData).SelectMany(trigger => trigger.Commands).OfType<ACName>().Any(command => command.Name == _config.Config.Name))
            {
                _billBoard = objectData;
                SetSign(_config.Config.TextItems[0]);
                _timer = AddTimer(new TimerT<TextRotatorBot>(RotationCallback, this, _config.Config.TextItems[0].Delay, 0));
                _timer.Start();
            }
        }

        void SetSign(TextRotatorConfigItem item)
        {
            _billBoard.Description = item.Text;
            Instance.ChangeObject(_billBoard);    
        }

        private void RotationCallback(TextRotatorBot state)
        {
            _timer.Change(_config.Config.TextItems[_rotationIndex].Delay, 0);
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
            // note that as we added the timer using the add timer method, from the base class we do not need to cleanup the timer.
            // it is handled by that base class.
        }
    }
}
