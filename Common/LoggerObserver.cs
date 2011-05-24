using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class LoggerObserver : IObserver // concrete observer
    {
        public LoggerObserver()
        {
            _name = "Logger";
        }
        public void Update(DataGatherer t)
        {
            Console.WriteLine(this.GetName() + " received message: " + t.GetMessage() + " from " + t.Name);
        }
        public string GetName()
        {
            return _name;
        }
        private readonly string _name;
    }
}
