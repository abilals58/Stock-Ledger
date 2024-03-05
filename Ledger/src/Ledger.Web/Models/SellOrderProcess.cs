using System.ComponentModel.DataAnnotations;

namespace Ledger.Ledger.Web.Models;

public class SellOrderProcess
{
    [Key]
    public int SellOrderProcessId { get; set; }
    public int SellOrderId { get; set; }
    public OrderStatus Status { get; set; }
    public int OrderNum { get; set; }
    public int StockId { get; set; }
    public double AskPrice { get; set; }

    public SellOrderProcess()
    {
        
    }

    public SellOrderProcess(int sellOrderProcessId, int sellOrderId, OrderStatus status, int stockId, double askPrice)
    {
        SellOrderProcessId = sellOrderProcessId;
        SellOrderId = sellOrderId;
        Status = status;
        OrderNum = sellOrderId;
        StockId = stockId;
        AskPrice = askPrice;
    }
    
    
    public override string ToString()
    {
        return $"SellOrderProcessId: {SellOrderProcessId}, SellOrderId: {SellOrderId}, Status: {Status}, OrderNum: {OrderNum}, StockId: {StockId}, AskPrice: {AskPrice}";
    }
}