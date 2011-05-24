using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public interface IObserver  // observer interface
    {
        void Update(DataGatherer t);
        string GetName();
    }
}
