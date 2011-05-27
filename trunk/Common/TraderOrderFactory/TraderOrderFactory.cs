// this factory pattern allows the creation of orders for the trader interface
namespace Common.TraderOrderFactory                             
{
    public abstract class TraderOrderCreator                    // abstract creator
    {
        public abstract TraderOrder MakeTradeOrder();
    }
    public class TraderBuyOrderCreator : TraderOrderCreator     // concrete creator
    {
        public override TraderOrder MakeTradeOrder()
        {
            return new TraderBuyOrder();
        }
    }
    public class TraderSellOrderCreator : TraderOrderCreator    // concrete creator
    {
        public override TraderOrder MakeTradeOrder()
        {
            return new TraderSellOrder();
        }
    }
    public abstract class TraderOrder                           // abstract product
    {
        private string _symbol;
        private string _purchaseType;
        private string _buyOrSell;
        private string _price;
        private string _quantity;
        protected TraderOrder(string buyOrSell)
        {
            _buyOrSell = buyOrSell;
        }
        public void SetSymbol(string symbol, string defaultSymbol)
        {
            if (symbol=="")
            {
                symbol = defaultSymbol;
            }
            _symbol = symbol;
        }
        public void SetPurchaseType(string purchaseType,string defaultPurchaseType)
        {
            if (purchaseType == "")
            {
                purchaseType = defaultPurchaseType;
            }
            _purchaseType = purchaseType;
        }
        public void SetBuyOrSell(string s)
        {
            _buyOrSell = s;
        }
        public void SetPrice(string s)
        {
            _price = s;
        }
        public void SetQuantity(string s)
        {
            _quantity = s;
        }
        public string GetSymbol()
        {
            return _symbol;
        }
        public string GetPurchaseType()
        {
            return _purchaseType;
        }
        public string GetBuyOrSell()
        {
            return _buyOrSell;
        }
        public string GetPrice()
        {
            return _price;
        }
        public string GetQuantity()
        {
            return _quantity;
        }
        public string GetOrderString()
        {
            return _symbol + "," + _purchaseType + "," + _buyOrSell + "," + _price + "," + _quantity;
        }
    }
    public class TraderBuyOrder : TraderOrder                   // concrete product 
    {
        public TraderBuyOrder() : base("B"){}
    }
    public class TraderSellOrder : TraderOrder                  // concrete product 
    {
        public TraderSellOrder() : base("S"){}
    }
}
