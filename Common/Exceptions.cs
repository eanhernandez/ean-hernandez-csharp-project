using System;
namespace Common
{
    [Serializable]
    // an exception to handle bad input from the CSV
    public abstract class TradingEngineException : Exception
    {
        protected TradingEngineException() 
        {
            DoNotifications();
        }
        protected TradingEngineException(string message) : base(message) { DoNotifications(); }
        protected TradingEngineException(string message, Exception inner) : base(message, inner) { DoNotifications(); }
        private void DoNotifications()
        {
            RtmDataGatherer rtm = new RtmDataGatherer("Error Handler");
            rtm.Attach(new LoggerObserver());
            rtm.Attach(new ScreenPrinterObserver());
            rtm.Attach(new EmailerObserver());
            rtm.SetMessage(Message);
            rtm.Notify();
        }
    }
    [Serializable]
    // an exception to handle bad input from the CSV
    public class BadOrderInput : TradingEngineException
    {
        public BadOrderInput()  { }
        public BadOrderInput(string message) : base(message) { }
        public BadOrderInput(string message, Exception inner) : base(message, inner) { }
    }
    [Serializable]
    // an exception to handle bad input from the OME
    public class BadTickerInput : TradingEngineException
    {
        public BadTickerInput()  { }
        public BadTickerInput(string message) : base(message) { }
        public BadTickerInput(string message, Exception inner) : base(message, inner) { }
    }
    [Serializable]
    // an exception to handle bad input from the OME
    public class CommsException : TradingEngineException
    {
        public CommsException() { }
        public CommsException(string message) : base(message) { }
        public CommsException(string message, Exception inner) : base(message, inner) { }
    }
}
