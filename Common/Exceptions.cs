using System;
namespace Common
{
    [Serializable()]
    // an exception to handle bad input from the CSV
    public abstract class TradingEngineException : System.Exception
    {
        protected TradingEngineException() : base() { }
        protected TradingEngineException(string message) : base(message) { }
        protected TradingEngineException(string message, System.Exception inner) : base(message, inner) { }
    }
    [Serializable()]
    // an exception to handle bad input from the CSV
    public class BadOrderInput : TradingEngineException
    {
        public BadOrderInput()  { }
        public BadOrderInput(string message) : base(message) { }
        public BadOrderInput(string message, System.Exception inner) : base(message, inner) { }
    }
    [Serializable()]
    // an exception to handle bad input from the OME
    public class BadTickerInputException : TradingEngineException
    {
        public BadTickerInputException()  { }
        public BadTickerInputException(string message) : base(message) { }
        public BadTickerInputException(string message, System.Exception inner) : base(message, inner) { }
    }
}
