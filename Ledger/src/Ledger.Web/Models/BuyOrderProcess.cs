namespace Ledger.Ledger.Web.Models;

public class BuyOrderProcess
{
    public int BuyOrderProcessId { get; set; }
    public int BuyOrderId { get; set; }
    public OrderStatus Status { get; set; }
    public int StockId { get; set; }
    public double BidPrice { get; set; }

    public BuyOrderProcess()
    {
        
    }

    public BuyOrderProcess(int buyOrderProcessId, int buyOrderId, OrderStatus status, int stockId, double bidPrice)
    {
        BuyOrderProcessId = buyOrderProcessId;
        BuyOrderId = buyOrderId;
        Status = status;
        StockId = stockId;
        BidPrice = bidPrice;
    }
    
    public override string ToString()
    {
        return $"BuyOrderProcessId: {BuyOrderProcessId}, BuyOrderId: {BuyOrderId}, Status: {Status}, StockId: {StockId}, BidPrice: {BidPrice}";
    }
}