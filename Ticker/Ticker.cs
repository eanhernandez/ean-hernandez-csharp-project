using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Common;
using System.Net.Sockets;
using System.Configuration;

namespace Ticker
{
    class Program
    {
        static void Main(string[] args)
        {
            // set up multicast 
            byte[] receiveBuffer = new byte[512];
            EndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            Socket mdcSocket = CommsTools.setUpMcastListenSocket(Convert.ToInt32(ConfigurationManager.AppSettings["receive_port"]));
            Console.WriteLine("Ticker Service Started - (Listening Using MultiCast)");	
            while (true)
            {
                int bytesReceived = mdcSocket.ReceiveFrom(receiveBuffer, ref endPoint);
                IPEndPoint mdpEndPoint = (IPEndPoint)endPoint;
                string s = Encoding.ASCII.GetString(receiveBuffer, 0, bytesReceived);
                //Console.WriteLine(s);
                Common.RtmDataGatherer rtm = new RtmDataGatherer("RTM");
                rtm.Attach(new LoggerObserver());
                rtm.Attach(new ScreenPrinterObserver());
                rtm.SetMessage(s);
                rtm.Notify();
            }
        }
    }
}
