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
}
