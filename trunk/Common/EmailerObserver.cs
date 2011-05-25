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
            if (MaskSingleton.Instance.ShouldThisObserverTakeAction(this))
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
