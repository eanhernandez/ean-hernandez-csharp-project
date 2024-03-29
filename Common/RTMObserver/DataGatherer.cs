﻿using System.Collections.Generic;

namespace Common.RTMObserver
{
    // this class is the abstract subject for the observer pattern used by the RTM
    // subsystem.
    public abstract class DataGatherer 
    {
        public DataGatherer(string name)
        {
             _observers= new List<IObserver>();
             this.Name = name;
        }
        public string GetMessage()
        {
            return _message;
        }
        public void SetMessage(string s)
        {
            _message = s;
        }
        private string _message;
        public void Attach(IObserver i)
        {
            _observers.Add(i);
        }
        public void Detach(IObserver i)
        {
            _observers.Remove(i);
        }
        public void Notify()
        {
            foreach (IObserver i in _observers)
            {
                i.Update(this);
            }
        }
        public string Name;
        private readonly List<IObserver> _observers;
        
    }
}
