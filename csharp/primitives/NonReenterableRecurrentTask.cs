using System.Timers;
using System.Threading;
using System;

namespace gvaduha.Framework
{
    public class NonReenterableRecurrentTask : IDisposable
    {
        private System.Timers.Timer _timer;
        private Action _action;
        private ManualResetEvent _idling = new ManualResetEvent(true);

        public NonReenterableRecurrentTask(double millisecondsInterval, Action task)
        {
            _timer = new System.Timers.Timer(millisecondsInterval);
            _timer.Elapsed += Tick;
            _action = task;
        }

        private void Tick(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Bump");
            if (_idling.WaitOne(0))
                Run();
        }

        private void Run()
        {
            _idling.Reset();
            _action();
            _idling.Set();
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        public void Dispose()
        {
            if (!disposedValue)
            {
                _timer.Stop();
                _timer.Dispose();
                disposedValue = true;
            }
        }
        #endregion
    }
}
