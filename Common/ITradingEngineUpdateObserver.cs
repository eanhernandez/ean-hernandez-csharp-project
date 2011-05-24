using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    interface ITradingEngineUpdateObserver  // observer interface
    {
        void Update(TradingEngineDataGatherer t);
    }
}
