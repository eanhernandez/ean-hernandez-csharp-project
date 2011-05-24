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
            bool b = Convert.ToBoolean(ConfigurationManager.AppSettings["ScreenPrinterPref"]);
            if (b)
            {
                Console.WriteLine(this.GetName() + " received message: " + t.GetMessage() + " from " + t.Name);
            }
        }
        public string GetName()
        {
            return _name;
        }
        private readonly string _name;
    }
}
