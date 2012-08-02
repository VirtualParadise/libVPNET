using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Threading;
using VpNet.Core;
using VpNet.NativeApi;

namespace VPNetExamples.Common
{
    internal abstract class BaseExampleBot : IDisposable
    {
        private enum ReconnectionType
        {
            World,
            Universe
        }

        private Instance _instance;
        internal Instance Instance { get { return _instance; } }
        private Timer _timer;

        private List<BaseExampleBot> _attachedBots;

        public abstract void Initialize();


        protected BaseExampleBot()
        {
            EnterUniverse();
        }

        void EnterUniverse()
        {
            if (_instance==null)
                _instance = new Instance();
            _instance.Connect(ConfigurationManager.AppSettings["server"], ushort.Parse(ConfigurationManager.AppSettings["serverPort"]));
            _instance.Login(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"], ConfigurationManager.AppSettings["botName"]);
            _instance.EventUniverseDisconnect += EventUniverseDisconnect;
            _instance.EventWorldDisconnect += EventWorldDisconnect;
            EnterWorld();
        }

        void EnterWorld()
        {
            _instance.Enter(ConfigurationManager.AppSettings["world"]);
            var position = ConfigurationManager.AppSettings["position"].Split(',');
            var rotation = ConfigurationManager.AppSettings["rotation"].Split(',');
            var ci = new CultureInfo("en-US");
            _instance.UpdateAvatar(float.Parse(position[0], ci), float.Parse(position[1], ci),
                                    float.Parse(position[2], ci),
                                    float.Parse(rotation[0], ci), float.Parse(rotation[1]));
            _timer = new Timer(AliveCallBack, null, 0, 0);
        }

        void DisconnectBots()
        {
            if (_attachedBots != null)
            {
                // signal all bots to disconnect.
                foreach (var bot in _attachedBots)
                {
                    // give bot some time to cleanup.
                    bot.Disconnect();
                }
            }
            // give main bot time to cleanup.
            Disconnect();
        }

        void Reconnect(object reconnectionType)
        {
            _timer.Dispose(); // dispose of wait timer or reconnect timer.
            // reconnect to universe.
            uint reconnectTime = 2000;
            try
            {
                switch ((ReconnectionType) reconnectionType)
                {
                    case ReconnectionType.World:
                        reconnectTime = uint.Parse(ConfigurationManager.AppSettings["worldReconnect"]);
                        EnterWorld();
                        break;
                    case ReconnectionType.Universe:
                        reconnectTime = uint.Parse(ConfigurationManager.AppSettings["universeReconnect"]);
                        EnterUniverse();
                        break;
                }
            }
            catch (VpException ex)
            {
                switch (ex.Reason)
                {
                    case ReasonCode.InvalidPassword:
                    case ReasonCode.VersionMismatch:
                    case ReasonCode.NotAllowed:
                    case ReasonCode.StringTooLong:
                        Console.WriteLine("Fatal error, reconnection disabled, reason {0}, exiting...", ex.Message);
                        Environment.Exit((int) ex.Reason);
                        return;
                    case ReasonCode.ConnectionError:
                        // go into universe reconnect mode incase we first got a world connect error.
                        reconnectionType = ReconnectionType.Universe;
                        break;
                    case ReasonCode.WorldNotFound:
                        // go into world reconnect mode incase we are still in universe reconnect mode.
                        reconnectionType = ReconnectionType.World;
                        break;

                }
                Console.WriteLine("Reconnection failed. trying again in {0}ms. Reason: {1}", reconnectTime,
                                    ex.Message);
                _timer = new Timer(Reconnect,reconnectionType,reconnectTime,0);
                return;
            }
            catch (AccessViolationException ex)
            {
                Console.WriteLine(ex.Message);
                reconnectionType = ReconnectionType.Universe;
                _timer = new Timer(Reconnect, reconnectionType, reconnectTime, 0);
                return;
            }

            if (_attachedBots != null)
            {
                foreach (var bot in _attachedBots)
                {
                    bot.Initialize();
                }
            }
            this.Initialize();
        }

        void EventWorldDisconnect(Instance sender)
        {
            DisconnectBots();
            Console.WriteLine("World connection lost, trying to re-enter world.");
            Reconnect(ReconnectionType.World);
        }

        void EventUniverseDisconnect(Instance sender)
        {
            DisconnectBots(); 
            Console.WriteLine("Universe connection lost, trying to reconnect.");
            Reconnect(ReconnectionType.Universe);
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
            _timer = new Timer(AliveCallBack, null, 30, 0);
        }

        public virtual void Dispose()
        {
            _timer.Dispose();
            _instance.Dispose();
        }

        /// <summary>
        /// Allow gracefull disconnect.
        /// </summary>
        public abstract void Disconnect();
    }
}
