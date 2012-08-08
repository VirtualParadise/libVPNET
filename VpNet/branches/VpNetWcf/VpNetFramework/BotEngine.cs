using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using VpNet.Core;
using VpNet.NativeApi;
using VpNetFramework.Common;
using VpNetFramework.Common.ActionInterpreter;

namespace VpNetFramework
{
    public abstract class BotEngine : IDisposable
    {
        private readonly BotEngineConfiguration _configuration;

        private enum ReconnectionType
        {
            World,
            Universe
        }

        private IInstance _instance;

        public Interpreter Interpreter
        {
            get { return _interpreter; }
        }

        internal IInstance Instance { get { return _instance; } }
        private Timer _timer;

        private List<BotEngine> _attachedBots;
        private List<ITimerT> _timers;

        public TimerT<TState> AddTimer<TState>(TimerT<TState> timer)
        {
            if (_timers == null)
                _timers = new List<ITimerT>();
            _timers.Add(timer);
            return timer;
        }

        private Interpreter _interpreter;


        public abstract void Initialize();


        protected BotEngine(BotEngineConfiguration configuration)
        {
            _configuration = configuration;
            _interpreter = new Interpreter(Assembly.GetAssembly(typeof(Interpreter)));
            EnterUniverse();
        }

        void EnterUniverse()
        {
            if (_instance == null)
                _instance = new Instance();
            _instance.Connect(_configuration.Server, _configuration.ServerPort);
            _instance.Login(_configuration.LoginUser, _configuration.LoginPassword, _configuration.BotName);
            _instance.EventUniverseDisconnect += EventUniverseDisconnect;
            _instance.EventWorldDisconnect += EventWorldDisconnect;
            EnterWorld();
        }

        void EnterWorld()
        {
            _instance.Enter(_configuration.World);

            _instance.UpdateAvatar(_configuration.Position.X, _configuration.Position.Y,
                                   _configuration.Position.Z, _configuration.Rotation.Y, _configuration.Rotation.Z);
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
                switch ((ReconnectionType)reconnectionType)
                {
                    case ReconnectionType.World:
                        reconnectTime = _configuration.ReconnecWorld.Milliseconds;
                        EnterWorld();
                        break;
                    case ReconnectionType.Universe:
                        reconnectTime = _configuration.ReconnectUniverse.Milliseconds;
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
                        Environment.Exit((int)ex.Reason);
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
                _timer = new Timer(Reconnect, reconnectionType, reconnectTime, 0);
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

        void DisposeTimers()
        {
            foreach (var timer in _timers)
            {
                timer.Dispose();
            }
            _timers.Clear();
        }

        void EventWorldDisconnect(IInstance sender)
        {
            DisposeTimers();
            DisconnectBots();
            Instance.ReleaseEvents();
            Console.WriteLine("World connection lost, trying to re-enter world.");
            Reconnect(ReconnectionType.World);
        }

        void EventUniverseDisconnect(IInstance sender)
        {
            DisposeTimers();
            DisconnectBots();
            Instance.ReleaseEvents();
            DisconnectBots();
            Console.WriteLine("Universe connection lost, trying to reconnect.");
            Reconnect(ReconnectionType.Universe);
        }

        protected BotEngine(IInstance instance)
        {
            _instance = instance;
        }

        public void AttachBot<T>() where T : BotEngine
        {
            if (_attachedBots == null)
                _attachedBots = new List<BotEngine>();

            var botInstance = (T)Activator.CreateInstance(typeof(T), new object[] { _instance });
            // share the action interpreter
            botInstance._interpreter = this._interpreter;
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
            DisconnectBots();
            _instance.Dispose();
        }

        /// <summary>
        /// Allow gracefull disconnect.
        /// </summary>
        public abstract void Disconnect();
    }
}
