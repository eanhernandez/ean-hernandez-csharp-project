using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Common;

namespace TraderTool
{
    class TraderTool
    {
        static void Main(string[] args)
        {
            string symbol = "";
            string purchaseType = "";
            string buyorsell = "";
            string price = "";
            string quantity = "";
            string order = "";

            Common.CommsTools.SetUpMCastSendSocket();
            Socket mdpSocket = CommsTools.SetUpMCastSendSocket();
            IPEndPoint mcastEp = new IPEndPoint(IPAddress.Parse("224.5.6.7"),
                Convert.ToInt32(ConfigurationManager.AppSettings["send_port"]));

            Console.WriteLine("Trader Tool");

            while (true)
            {
                Console.Write("hit enter to begin a trade.");
                Console.ReadLine();


                Console.Write("Symbol [MSFT]: ");
                symbol = Console.ReadLine();
                if (symbol == "")
                {
                    symbol = "MSFT";
                    Console.WriteLine(symbol);
                }

                Console.Write("purchase type [Regular]:");
                purchaseType = Console.ReadLine();
                if (purchaseType == "")
                {
                    purchaseType = "Regular";
                    Console.WriteLine(purchaseType);
                }

                Console.Write("Buy or Sell [B|S]: ");
                buyorsell = Console.ReadLine();

                Console.Write("price: ");
                price = Console.ReadLine();

                Console.Write("quantity: ");
                quantity = Console.ReadLine();

                order = symbol + "," + purchaseType + "," + buyorsell + "," + price + "," + quantity;

                Console.WriteLine("your order: " + order);
                Console.Write("OK? [Y|n]");
                if (Console.ReadLine() == "")
                {
                    if (Common.Tools.ValidateOrderRequest(order))
                    {
                        CommsTools.sendOrderDataToOME(order, mdpSocket, mcastEp);
                        Console.WriteLine("Order sent!");
                    }
                    else
                    {
                        Console.WriteLine("bad input, no order sent.");
                    }
                }
                Console.WriteLine("hit enter to continue.");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}
