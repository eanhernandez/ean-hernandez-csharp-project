using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Common;
using System.Net.Sockets;
using System.Configuration;
using Common.RTMObserver;

namespace Ticker
{
    class Program
    {
        static void Main(string[] args)
        {
            // set up multicast 
            byte[] receiveBuffer = new byte[512];
            EndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            Socket mdcSocket = CommsTools.SetUpMcastListenSocket(Convert.ToInt32(ConfigurationManager.AppSettings["receive_port"]));
            Console.WriteLine("Ticker Service Started - (Listening Using MultiCast)");
            RtmDataGatherer rtm = new RtmDataGatherer("Ticker RTM");
            rtm.Attach(new LoggerObserver());
            rtm.Attach(new ScreenPrinterObserver());
           
            while (true)
            {
                int bytesReceived = mdcSocket.ReceiveFrom(receiveBuffer, ref endPoint);
                IPEndPoint mdpEndPoint = (IPEndPoint)endPoint;
                string s = Encoding.ASCII.GetString(receiveBuffer, 0, bytesReceived);
               
                rtm.SetMessage(s);
                rtm.Notify();
            }
        }
    }
}
