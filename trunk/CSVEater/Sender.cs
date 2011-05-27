using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.Configuration;
using Common;
using Common.RTMObserver;

namespace CSVEater
{
	class MDP
    {
		[STAThread]
		static void Main(string[] args)
		{
            string line;

            // this implementation of the observer pattern creates a real time
            // monitor, and attaches to logger,and screen printer concrete observers.  
            // the emailer concrete observer is left out here.
            RtmDataGatherer rtm = new RtmDataGatherer("CSV Eater RTM");
            rtm.Attach(new LoggerObserver());
            rtm.Attach(new ScreenPrinterObserver());

            // setting up to send multicast
            Socket mdpSocket = CommsTools.SetUpMCastSendSocket();
            IPEndPoint mcastEp = new IPEndPoint(IPAddress.Parse("224.5.6.7"),
                Convert.ToInt32(ConfigurationManager.AppSettings["send_port"]));

            Console.WriteLine("CSV Eater Service Started - (Sending Using MultiCast)");
            Thread.Sleep(3000);  // relax a moment while the receiver starts up
            
            // read from the csv
            var stream = File.OpenRead(
                ConfigurationManager.AppSettings["csvpath"]);
            var streamReader = new StreamReader(stream);
            
            while ((line = streamReader.ReadLine()) != null)
            {
                try
                {
                    // this method will throw the BadOrderInput exception if the 
                    // order format doesn't match the required pattern
                    CommsTools.sendOrderDataToOME(line, mdpSocket, mcastEp);
                    rtm.SetMessage("sending: " + line);
                    rtm.Notify();
                }
                catch (BadOrderInput e)
                {
                    // simply skipping and going on to the next CSV line, the exception
                    // itself logs the error
                }

                // this just keeps the orders from all zipping by too fast to see
                Thread.Sleep(Convert.ToInt32(ConfigurationManager.AppSettings["order_send_delay"]));
            }

            // telling receiver we're all done
            CommsTools.SendMCastData("-1,-1,-1,-1,-1", mdpSocket, mcastEp);  // send quit signal
            Console.WriteLine("reached end of file, sent quit signal");
            mdpSocket.Close();
		    Environment.Exit(0);
		}
	}
}
