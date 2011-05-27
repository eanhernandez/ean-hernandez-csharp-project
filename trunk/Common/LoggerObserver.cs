using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;

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
            if (MaskSingleton.Instance.ShouldThisObserverTakeAction(this))
            {
                StreamWriter w = File.AppendText(
                    Convert.ToString(ConfigurationManager.AppSettings["logfile"]) 
                    + Process.GetCurrentProcess().ProcessName + ".log");
                w.WriteLine(this.GetName() + " received message: " + t.GetMessage() + " from " + t.Name);
                w.Flush();
                w.Close();
            }
        }
        public string GetName()
        {
            return _name;
        }
        private readonly string _name;
    }
}
