using System;
using System.Collections;
using Common;
using OME;

namespace OME.Storage
{
	public class LeafContainer : Container , IEnumerable , IEnumerator
	{
		private int rowPos = -1;
		//The internal implementation of order is based on ArrayList
		//but remember based on performance critieria this implementation
		//can be easily changed without affecting the business component code
		ArrayList orderDataStore = ArrayList.Synchronized(new ArrayList());
		public LeafContainer(OrderBook oBook,string name,Container parent)
		:base(oBook,name,parent)
		{
		}
		public override IEnumerator GetEnumerator()
		{
			Reset();
			return this;
		}
		public override void ProcessOrder(Order newOrder)
		{
			//Access the buy order book of this instrument
			Container buyBook = parentContainer.ChildContainers["B"] ;
			//Access the sell order book of this instrument
			Container sellBook = parentContainer.ChildContainers["S"] ;
			//create a event arg object containing reference to newly created
			//order along with reference to buy and sell order book
			OrderEventArgs orderArg = new OrderEventArgs(newOrder,buyBook,sellBook);
			//Invoke the OrderBeforeInsert event which will also notify 
			//the matching business component which will then perform 
			//its internal matching 
			//the order becomes active in this stage
			orderBook.OnOrderBeforeInsert(orderArg);
			//Check the quantity of the newly created order
			//because if the order has been successfully matched by matching
			//business component then quantity will be 0
			if ( newOrder.Quantity > 0 ) 
			{
				//If order is partially or not at all matched 
				//then it is inserted in the order collection
				orderDataStore.Add(newOrder);
				//Re-sort the order collection because of addition
				//of new order
				orderDataStore.Sort(orderBook.OrderPriority);
                TickerHelper th = new TickerHelper();
                th.Attach(new LoggerObserver());
                th.Attach(new ScreenPrinterObserver());
                th.SendTickerData(orderArg);
				//Invoke the OrderInsert event 
				//which will again notify the matching business component
				//the order becomes passive in this stage
				orderBook.OnOrderInsert(orderArg);
			}
		}
		public void Reset()
		{
			rowPos=-1;
		}
		public object Current
		{
			get{return orderDataStore[rowPos];}
		}
		public bool MoveNext()
		{
			rowPos++;
			while(rowPos < orderDataStore.Count)
			{
				Order curOrder = orderDataStore[rowPos] as Order;
				if ( curOrder.Quantity == 0 ) 
					orderDataStore.RemoveAt(rowPos);
				else
					return true;
			}
			Reset();
			return false;
		}
	}
}
