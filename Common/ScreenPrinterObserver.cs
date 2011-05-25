using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Common
{
    public class ScreenPrinterObserver : IObserver // concrete observer
    {
        public ScreenPrinterObserver()
        {
            _name = "ScreenPrinter";
        }
        public void Update(DataGatherer t)
        {
            if (MaskSingleton.Instance.ShouldThisObserverTakeAction(this))
            {
                Console.WriteLine("(sp): " + t.GetMessage() + " from " + t.Name);
            }
        }
        public string GetName()
        {
            return _name;
        }
        private readonly string _name;
    }
}
