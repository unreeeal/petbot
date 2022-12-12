using NLog;
using System;
using System.Timers;
namespace PetBot
{
    /// <summary>
    /// Needed to pass to the bot watcher
    /// </summary>
    public class Spyer : IDisposable
    {
        
       private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public bool ContinueOnFail { get; set; } 
        public bool ContinueOnSuccess { get; set; } = true;

        private Timer _timer;
        private Func<string> _function;

        public event Action<string> OnReceivedData;
           

        public Spyer(TimeSpan interval,Func<string> func)
        {
            _function = func;
            
            _timer = new Timer();
            _timer.Interval = interval.TotalMilliseconds;
            _timer.Elapsed += (sender, e) => Run();

        }


        public void Start()
        {
            _timer.Start();

        }
        public void Stop()
        {
            _timer.Stop();

        }

        public bool IsRunning() => _timer.Enabled;
        //public void SetActionToSendResult(Action<string> action) => _actionToSendTheResult = action;




/// <summary>
/// Starts the spyer
/// </summary>
        private void Run()
        {
            try
            {
                string res = _function();
                if (!string.IsNullOrEmpty(res))
                {
                    try
                    {
                        OnReceivedData?.Invoke(res);
                    }
                    catch (Exception ex)
                    {

                        _logger.Error(ex,"Fail exectuing onRecievedData");
                    }
                    if (!ContinueOnSuccess)
                        Stop();
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Fail exectuing main spyer function");
                if (!ContinueOnFail)
                    Stop();
            }


        }

        public void Dispose()
        {
            _timer.Stop();
            _timer.Dispose();

        }
    }
}
