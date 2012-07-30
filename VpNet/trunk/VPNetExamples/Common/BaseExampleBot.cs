using System;
using System.Configuration;
using System.Threading;
using VpNet.Core;

namespace VPNetExamples.Common
{
    internal abstract class BaseExampleBot : IDisposable
    {
        private Instance _instance;
        internal Instance Instance { get { return _instance; } }
        private Timer _timer;

        protected BaseExampleBot()
        {
            _instance = new Instance();
            _instance.Connect(ConfigurationManager.AppSettings["server"], ushort.Parse(ConfigurationManager.AppSettings["serverPort"]));
            _instance.Login(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"], ConfigurationManager.AppSettings["botName"]);
            _instance.Enter(ConfigurationManager.AppSettings["world"]);
            _timer = new Timer(AliveCallBack,null,0,1000);
        }

        private void AliveCallBack(object state)
        {
            _instance.Wait(0);
        }

        public virtual void Dispose()
        {
            _timer.Dispose();
            _instance.Dispose();
        }
    }
}
