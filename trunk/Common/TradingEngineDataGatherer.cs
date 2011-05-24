using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    abstract class TradingEngineDataGatherer // abstract subject
    {
        public TradingEngineDataGatherer(string updateName)
        {
             _observers= new List<ITradingEngineUpdateObserver>();
            this._updateName = updateName;
        }
        public void Attach(ITradingEngineUpdateObserver i)
        {
            _observers.Add(i);
        }
        public void Detach(ITradingEngineUpdateObserver i)
        {
            _observers.Remove(i);
        }
        public void Notify()
        {
            foreach (ITradingEngineUpdateObserver i in _observers)
            {
                i.Update(this);
            }
        }
        private string _updateName;
        private List<ITradingEngineUpdateObserver> _observers;
    }
}
