using System.ComponentModel.DataAnnotations;

namespace Ledger.Ledger.Web.Models
{
    public class SellOrderMatch
    {
        public int SellOrderId { get; set; }
        public int BuyOrderId { get; set; }

        public SellOrderMatch()
        {
            
        }

        public SellOrderMatch(int sellOrderId, int buyOrderId)
        {
            SellOrderId = sellOrderId;
            BuyOrderId = buyOrderId;
        }
        
        public override string ToString()
        {
            return $"SellOrderId: {SellOrderId}, BuyOrderId: {BuyOrderId}";
        }
    }
}