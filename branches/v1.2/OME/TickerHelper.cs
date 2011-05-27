using System;
using System.Collections.Generic;
using System.Text;
using OME;
using OME.Storage;
using Common;
using System.Net.Sockets;
using System.Net;
using System.Configuration;

namespace OME
{ 
    class TickerHelper : DataGatherer
    {
        public TickerHelper() : base("TickerHelper")
        {}
        public  void SendTickerData(OrderEventArgs e)
        {
            double bestSellPrice = 0;
            double bestBuyPrice = 0;
            string instrument = "";
            foreach (Order s in e.SellBook) 
            {
                bestSellPrice = s.Price;
                instrument = s.Instrument;
                break; 
            }
            foreach (Order b in e.BuyBook) 
            {
                bestBuyPrice = b.Price; 
                instrument = b.Instrument;
                break; 
            }

            string bestBuyString = "-";
            string bestSellString = "-";
            if (bestBuyPrice != 0){ bestBuyString = bestBuyPrice.ToString(); }
            if (bestSellPrice != 0){ bestSellString = bestSellPrice.ToString(); }

            // set up multicast send for ticker
            Socket tickerSocket = CommsTools.SetUpMCastSendSocket();
            IPEndPoint tickerEP = new IPEndPoint(IPAddress.Parse("224.5.6.7"),
                Convert.ToInt32(ConfigurationManager.AppSettings["ticker_broadcast_port"]));
            try
            { 
                CommsTools.SendTradeDataToTicker(instrument + " " + bestBuyString + "/" + bestSellString, tickerSocket, tickerEP);
            }
            catch (BadTickerInput bte)
            {
                // not neccessary to do anything, just let next order go to ticker
            }
        }
    }
}
