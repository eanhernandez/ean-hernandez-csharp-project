using System;

namespace Common.RTMObserver
{
    // this concrete observer provides the RTM system with a means to log data to 
    // the screen.

    public class ScreenPrinterObserver : IObserver // concrete observer
    {
        public ScreenPrinterObserver()
        {
            _name = "ScreenPrinter";
        }
        public void Update(DataGatherer t)
        {
            if (MaskSingleton.MaskSingleton.Instance.ShouldThisObserverTakeAction(this))
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
