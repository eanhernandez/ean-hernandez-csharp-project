using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
            if (MaskSingleton.Instance.ShouldThisObserverTakeAction(this))
            {
                StreamWriter w = File.AppendText(
                    Convert.ToString(ConfigurationManager.AppSettings["logfile"]) + _name + ".log"
                    );
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
