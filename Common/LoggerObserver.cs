﻿using System;
using System.Collections.Generic;
using System.Configuration;
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
                Console.WriteLine(this.GetName() + 
                    " received message: " + t.GetMessage() + " from " + t.Name);
            }
        }
        public string GetName()
        {
            return _name;
        }
        private readonly string _name;
    }
}