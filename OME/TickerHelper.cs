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
    class TickerHelper
    {
        public static void SendTickerData(OrderEventArgs e)
        {
            double bestSellPrice = 0;
            double BestBuyPrice = 0;
            string instrument = "";
            foreach (Order s in e.SellBook) 
            {
                bestSellPrice = s.Price;
                instrument = s.Instrument;
                break; 
            }
            foreach (Order b in e.BuyBook) 
            {
                BestBuyPrice = b.Price; 
                instrument = b.Instrument;
                break; 
            }

            string bestBuyString = "-";
            string bestSellString = "-";
            if (BestBuyPrice != 0){ bestBuyString = BestBuyPrice.ToString(); }
            if (bestSellPrice != 0){ bestSellString = bestSellPrice.ToString(); }

            // set up multicast send for ticker
            Socket tickerSocket = CommsTools.SetUpMCastSendSocket();
            IPEndPoint tickerEP = new IPEndPoint(IPAddress.Parse("224.5.6.7"),
                Convert.ToInt32(ConfigurationManager.AppSettings["ticker_broadcast_port"]));
            try
            {
                CommsTools.SendMCastData(instrument + " " + bestBuyString + "/" + bestSellString, tickerSocket, tickerEP);
            }
            catch (BadTickerInputException bte)
            {
                Console.WriteLine("boom");
                Common.RtmDataGatherer rtm = new RtmDataGatherer("RTM");
                rtm.Attach(new LoggerObserver());
                rtm.Attach(new ScreenPrinterObserver());
                rtm.SetMessage(bte.Message);
                rtm.Notify();
            }
            
        }
    }
}
