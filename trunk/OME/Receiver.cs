using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using Common;
using System.Configuration;
using Common.RTMObserver;

namespace OME
{
	class MDC
	{
        static OME.BizDomain setUpEquityDomain()
        {
            OME.BizDomain equityDomain;
            equityDomain = new OME.BizDomain("Equity Domain", new string[] { "MSFT" });
            equityDomain.OrderBook.OrderPriority = new EquityMatchingEngine.PriceTimePriority();
            EquityMatchingEngine.EquityMatchingLogic equityMatchingLogic = new EquityMatchingEngine.EquityMatchingLogic(equityDomain);

            // setting up equityMatchingLogic to use observer 
            equityMatchingLogic.Attach(new LoggerObserver());
            equityMatchingLogic.Attach(new EmailerObserver());
            equityMatchingLogic.Attach(new ScreenPrinterObserver());

            return equityDomain;
        }
        
        [STAThread]
		static void Main(string[] args)
		{
			Console.WriteLine("OME Service Started - (Listening Using MultiCast)");	
			
            // set up multicast receive
            byte[] receiveBuffer = new byte[512];
			EndPoint endPoint = new IPEndPoint(IPAddress.Any,0);
            Socket mdcSocket = CommsTools.SetUpMcastListenSocket(
                Convert.ToInt32(ConfigurationManager.AppSettings["receive_port"]));

            // using the observer pattern here to log what goes on in main
            RtmDataGatherer rtm = new RtmDataGatherer("OME Receiver RTM");
            rtm.Attach(new LoggerObserver());
            rtm.Attach(new EmailerObserver());
            rtm.Attach(new ScreenPrinterObserver());

            rtm.Notify();   

            // set up OME
            OME.BizDomain equityDomain = setUpEquityDomain();
            equityDomain.Start();
            
            // loop until we get a quit signal
			while (true)
            {
			    int bytesReceived = mdcSocket.ReceiveFrom(receiveBuffer,ref endPoint);
			    IPEndPoint mdpEndPoint = (IPEndPoint)endPoint;
			    string inboundOrderText = Encoding.ASCII.GetString(receiveBuffer,0,bytesReceived);
			    
                rtm.SetMessage("Order Received : " + inboundOrderText);
                rtm.Notify();
                
                var array = inboundOrderText.Split(',');
                if (array[0] == "-1") { break; } // quit signal

                try
                {
                    equityDomain.SubmitOrder("MSFT", new EquityMatchingEngine.EquityOrder(
                        array[0], array[1], array[2], Convert.ToDouble(array[3]),
                        Convert.ToInt32(array[4])));
                }
                catch(BadOrder e)
                {
                    // nothing to do here, as this order will just be skipped,
                    // and the exception will make the neccessary notifications.
                }
            }
			mdcSocket.Close();
            Console.WriteLine("received quit signal");
			Console.ReadLine();
		}
	}
}
