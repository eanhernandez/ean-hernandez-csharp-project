using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using Common;
using Common.TraderOrderFactory;

namespace TraderTool
{
    class TraderTool
    {
        // this code uses the factory pattern to create new orders for sending on the the OME.
        static void Main(string[] args)
        {
            Common.CommsTools.SetUpMCastSendSocket();
            Socket mdpSocket = CommsTools.SetUpMCastSendSocket();
            IPEndPoint mcastEp = new IPEndPoint(IPAddress.Parse("224.5.6.7"),
                Convert.ToInt32(ConfigurationManager.AppSettings["send_port"]));
            TraderOrderCreator tocBuy = new TraderBuyOrderCreator();
            TraderOrderCreator tocSell = new TraderSellOrderCreator();
            TraderOrder to;
            string order = "";
            Console.WriteLine("Trader Tool");

            while (true)
            {
                Console.Write("hit enter to begin a trade.");
                Console.ReadLine();

                Console.Write("Buy or Sell [B|S]: ");
                string buyorsell = Console.ReadLine();

                if (buyorsell == "B")
                {
                    // using factory pattern to make orders
                    to = tocBuy.MakeTradeOrder();
                }
                else if (buyorsell == "S")
                {
                    // using factory pattern to make orders
                    to = tocSell.MakeTradeOrder();
                }
                else
                {
                    Console.WriteLine("must be B or S, hit enter to continue.");
                    continue;
                }

                Console.Write("Symbol [MSFT]: ");
                to.SetSymbol(Console.ReadLine(),"MSFT");
                Console.WriteLine(to.GetSymbol());

                Console.Write("purchase type [Regular]:");
                to.SetPurchaseType(Console.ReadLine(),"Regular");
                Console.WriteLine(to.GetPurchaseType());

                Console.Write("price: ");
                to.SetPrice(Console.ReadLine());

                Console.Write("quantity: ");
                to.SetQuantity(Console.ReadLine());

                Console.WriteLine("your order: " + to.GetOrderString());
                Console.Write("OK? [Y|n]");
                string temp = Console.ReadLine();
                
                if (temp == "" || temp == "Y")
                {
                    try
                    {
                        CommsTools.sendOrderDataToOME(to.GetOrderString(), mdpSocket, mcastEp);
                        Console.WriteLine("Order sent!");
                    }
                    catch (BadOrderInput e)
                    {
                        Console.WriteLine("bad input, no order sent.");
                    }
                }
                to = null;
                Console.WriteLine("hit enter to continue.");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}
