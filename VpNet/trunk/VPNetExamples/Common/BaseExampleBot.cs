using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Threading;
using VpNet.Core;

namespace VPNetExamples.Common
{
    internal abstract class BaseExampleBot : IDisposable
    {
        private Instance _instance;
        internal Instance Instance { get { return _instance; } }
        private Timer _timer;

        private List<BaseExampleBot> _attachedBots;

        public abstract void Initialize();


        protected BaseExampleBot()
        {
            _instance = new Instance();
            _instance.Connect(ConfigurationManager.AppSettings["server"], ushort.Parse(ConfigurationManager.AppSettings["serverPort"]));
            _instance.Login(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"], ConfigurationManager.AppSettings["botName"]);
            _instance.Enter(ConfigurationManager.AppSettings["world"]);

            var position = ConfigurationManager.AppSettings["position"].Split(',');
            var rotation = ConfigurationManager.AppSettings["rotation"].Split(',');
            var ci = new CultureInfo("en-US");
            _instance.UpdateAvatar(float.Parse(position[0], ci), float.Parse(position[1], ci),float.Parse(position[2], ci), 
                float.Parse(rotation[0], ci), float.Parse(rotation[1]));
            _timer = new Timer(AliveCallBack,null,0,0);
      
        }

        protected BaseExampleBot(Instance instance)
        {
            _instance = instance;
        }

        public void AttachBot<T>() where T : BaseExampleBot
        {
            if (_attachedBots == null)
                _attachedBots = new List<BaseExampleBot>();

            var botInstance = (T) Activator.CreateInstance(typeof (T), new object[] {_instance});
            botInstance.Initialize();
            _attachedBots.Add(botInstance);
        }

        private void AliveCallBack(object state)
        {
            _instance.Wait(0);
            _timer = new Timer(AliveCallBack, null, 1000, 0);
        }

        public virtual void Dispose()
        {
            _timer.Dispose();
            _instance.Dispose();
        }
    }
}
