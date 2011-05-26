using System;

namespace Common
{
    [Serializable()]
    // an exception to handle bad input from the CSV
    public class BadOrderInput : System.Exception
    {
        public BadOrderInput() : base() { }
        public BadOrderInput(string message) : base(message) { }
        public BadOrderInput(string message, System.Exception inner) : base(message, inner) { }
    }
    [Serializable()]
    // an exception to handle bad input from the OME
    public class BadTickerInputException : System.Exception
    {
        public BadTickerInputException() : base() { }
        public BadTickerInputException(string message) : base(message) { }
        public BadTickerInputException(string message, System.Exception inner) : base(message, inner) { }
    }


}
