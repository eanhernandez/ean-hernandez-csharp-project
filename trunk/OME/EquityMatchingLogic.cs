using System;
using Common;
using OME;
using OME.Storage;
using System.Collections;

namespace EquityMatchingEngine
{
	public class EquityMatchingLogic
	{
		public EquityMatchingLogic(BizDomain bizDomain)
		{
			//Hook up to active order event of the order book
			bizDomain.OrderBook.OrderBeforeInsert +=new OrderEventHandler(OrderBook_OrderBeforeInsert);			
		}

		private void OrderBook_OrderBeforeInsert(object sender, OrderEventArgs e)
		{
			//Check buy/sell leg of the order
			//as the matching logic is different 

            if (e.Order.BuySell == "B")
            { MatchBuyLogic(e); }
            else
            { MatchSellLogic(e); }


		}

		private void MatchBuyLogic(OrderEventArgs e)
		{
			//since the order to be matched is a buy order
			//therefore start iterating orders in sell order book
			foreach(Order curOrder in e.SellBook)
			{
				//If the current price of sell order price is less 
				//than the price of buy order then it is a best match
				if ( curOrder.Price <= e.Order.Price && e.Order.Quantity > 0 )
				{
					//Generate Trade

					//get the buy order quantity
					int quantity = e.Order.Quantity;

                    // (eh) track shares exchanged in each trade
                    int original_buy_quantity = e.Order.Quantity;
                    int original_sell_quantity = curOrder.Quantity;

					//subtract the buy order quantity from current sell order quantity
					curOrder.Quantity = curOrder.Quantity - e.Order.Quantity;

                    // (eh) removed bc it wasn't decrementing the active order quantity
                    //e.Order.Quantity = e.Order.Quantity - quantity;

                    // (eh) this should upate the order quantity correctly
                    e.Order.Quantity = original_buy_quantity - original_sell_quantity;

                    // (eh) to track shares exchanged in each trade
                    int sold_last_trade = original_buy_quantity - e.Order.Quantity;

                    // (eh) to show trades as they occur
                    //Console.WriteLine("Match Buy found..Generate Trade:" +
                    //" buy " +
                    //curOrder.Instrument + " " +
                    //sold_last_trade + " @" +
                    //e.Order.Price
                    //curOrder.Price
                    // );
                    Common.RtmDataGatherer rtm = new RtmDataGatherer("RTM");
                    rtm.Attach(new LoggerObserver());
                    rtm.Attach(new EmailerObserver());
                    rtm.Attach(new ScreenPrinterObserver());
                    rtm.SetMessage("Match Buy found..Generate Trade:" +
                    " buy " +
                    curOrder.Instrument + " " +
                    sold_last_trade + " @" +
                    //e.Order.Price
                    curOrder.Price);
                    rtm.Notify();

                    //assign the remaining quantity to buy order
                    

                }
				else
				{
					break;
				}
			}
		}

		private void MatchSellLogic(OrderEventArgs e)
		{
			//since the order to be matched is a sell order
			//therefore start iterating orders in buy order book
			foreach(Order curOrder in e.BuyBook)
			{
				//If the current price of buy order is greater 
				//than the price of sell order then it is a best match 
				if ( curOrder.Price >= e.Order.Price && e.Order.Quantity > 0 )
				{
					//Generate Trade
                   //get the sell order quantity
					int quantity = curOrder.Quantity;

                    // (eh) to track shares exchanged in each trade
                    int original_buy_quantity = curOrder.Quantity;
                    int original_sell_quantity = e.Order.Quantity;

                    //subtract the sell order quantity from current buy order quantity
					curOrder.Quantity = curOrder.Quantity - e.Order.Quantity;

					//assign the remaining quantity to sell order
                    // (eh) removed bc it wasn't decrementing the active order quantity
					//e.Order.Quantity = e.Order.Quantity - quantity;
                    // (eh) this should update it correctly
                    e.Order.Quantity = original_sell_quantity - original_buy_quantity;

                    // (eh) to track shares exchanged in each trade
                    int sold_last_trade = original_sell_quantity - e.Order.Quantity;

                    // (eh) to show trades as they occur
                    Console.WriteLine("Match Sell found..Generate Trade:" +
                    " sold " +
                    curOrder.Instrument + " " +
                    sold_last_trade + " @" +
                    //e.Order.Price
                    curOrder.Price
                    );
                
                }
				else
				{
					break;
				}
			}
		}
	}
}
