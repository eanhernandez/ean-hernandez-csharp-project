using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using OME.Storage;
using Common;
using System.Configuration;

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
            return equityDomain;
        }
        
        [STAThread]
		static void Main(string[] args)
		{
			Console.WriteLine("OME Service Started - (Listening Using MultiCast)");	
			
            // set up multicast receive
            byte[] receiveBuffer = new byte[512];
			EndPoint endPoint = new IPEndPoint(IPAddress.Any,0);
            Socket mdcSocket = CommsTools.setUpMcastListenSocket(
                Convert.ToInt32(ConfigurationManager.AppSettings["receive_port"]));

            Common.RtmDataGatherer rtm = new RtmDataGatherer("RTM");
            rtm.Attach(new LoggerObserver());
            rtm.Attach(new EmailerObserver());
            rtm.Attach(new ScreenPrinterObserver());
            rtm.SetMessage("kicking off OME");
            rtm.Notify();   

            // set up OME
            OME.BizDomain equityDomain = setUpEquityDomain();
            equityDomain.Start();

            // loop until we get a quit signal
			while (true)
            {
			    int bytesReceived = mdcSocket.ReceiveFrom(receiveBuffer,ref endPoint);
			    IPEndPoint mdpEndPoint = (IPEndPoint)endPoint;
			    string mktPrice = Encoding.ASCII.GetString(receiveBuffer,0,bytesReceived);
			    
                rtm.SetMessage("Order Received : " + mktPrice);
                rtm.Notify();
                
                var array = mktPrice.Split(',');
                if (array[0] == "-1") { break; } // quit signal
                
                equityDomain.SubmitOrder("MSFT", new EquityMatchingEngine.EquityOrder(
                    array[0], array[1], array[2], Convert.ToDouble(array[3]), 
                    Convert.ToInt32(array[4])));
            }

			mdcSocket.Close();
            Console.WriteLine("received quit signal");
			Console.ReadLine();
		}

		public static void DisableMulticastLoopBack(Socket sockInstance)
		{
			sockInstance.SetSocketOption(SocketOptionLevel.IP,SocketOptionName.MulticastLoopback,0);
		}
	}
}
