using System;
using System.Threading;

namespace VpNetFramework.Common
{
    public class TimerT<TState> : ITimerT
    {
        private readonly TimerCallbackT _timerCallbackT;
        private readonly TState _state;
        private readonly long _dueTime;
        private readonly long _period;
        private Timer _timer;

        public delegate void TimerCallbackT(TState state);

        public TimerT(TimerCallbackT timerCallbackT, TState state, long dueTime, long period)
        {
            _timerCallbackT = timerCallbackT;
            _state = state;
            _dueTime = dueTime;
            _period = period;
        }

        private void Callback(object state)
        {
            _timerCallbackT((TState) state);
        }

        public void Dispose()
        {
            
            if (_timer != null) _timer.Dispose();
        }

        public void Change(long dueTime, long period)
        {
            _timer.Change(dueTime, period);
        }

        public void Start()
        {
            _timer = new Timer(Callback, _state, _dueTime, _period);
        }
    }

    public interface ITimerT : IDisposable
    {
        void Change(long dueTime, long period);
        void Start();
    }
}
