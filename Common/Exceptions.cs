using System;

namespace Common
{
    [Serializable()]
    public class BadOrderInput : System.Exception
    {
        public BadOrderInput() : base() { }
        public BadOrderInput(string message) : base(message) { }
        public BadOrderInput(string message, System.Exception inner) : base(message, inner) { }
    }
}
