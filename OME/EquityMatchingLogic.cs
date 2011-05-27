using System;
using Common;
using Common.RTMObserver;
using OME;
using OME.Storage;
using System.Collections;

namespace EquityMatchingEngine
{
	public class EquityMatchingLogic : DataGatherer
	{
		public EquityMatchingLogic(BizDomain bizDomain) : base("equity matching logic")
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
			foreach(Order curOrder in e.SellBook)
			{
				if ( curOrder.Price <= e.Order.Price && e.Order.Quantity > 0 )
				{
					//Generate Trade

					int quantity = e.Order.Quantity;
                    int originalBuyQuantity = e.Order.Quantity;
                    int originalSellQuantity = curOrder.Quantity;
					curOrder.Quantity = curOrder.Quantity - e.Order.Quantity;
                    e.Order.Quantity = originalBuyQuantity - originalSellQuantity;
                    int sold_last_trade = originalBuyQuantity - e.Order.Quantity;

                    SetMessage("Match Buy found..Generate Trade:" +
                    " buy " +
                    curOrder.Instrument + " " +
                    sold_last_trade + " @" +
                    //e.Order.Price
                    curOrder.Price);
                    Notify();
                }
				else
				{
					break;
				}
			}
		}
		private void MatchSellLogic(OrderEventArgs e)
		{
			foreach(Order curOrder in e.BuyBook)
			{
				if ( curOrder.Price >= e.Order.Price && e.Order.Quantity > 0 )
				{
					int quantity = curOrder.Quantity;

                    int originalBuyQuantity = curOrder.Quantity;
                    int originalSellQuantity = e.Order.Quantity;

					curOrder.Quantity = curOrder.Quantity - e.Order.Quantity;
                    e.Order.Quantity = originalSellQuantity - originalBuyQuantity;
                    int sold_last_trade = originalSellQuantity - e.Order.Quantity;

                    SetMessage("Match Sell found..Generate Trade:" +
                    " sold " +
                    curOrder.Instrument + " " +
                    sold_last_trade + " @" +
                    curOrder.Price
                        );
                    Notify();
                }
				else
				{
					break;
				}
			}
		}
	}
}
