using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Common
{
    public class EmailerObserver : IObserver // concrete observer
    {
        public EmailerObserver()
        {
            _name = "Emailer";
        }
        public void Update(DataGatherer t)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings[_name + "Pref"]))
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
