using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class ScreenPrinterObserver : IObserver // concrete observer
    {
        public ScreenPrinterObserver()
        {
            _name = "Screen Printer";
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
