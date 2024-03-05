using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ledger.Ledger.Web.Models
{
    public class Transaction // This class refers to the transaction object which is created after a trade operation and stores all the related information about that operation.
                            // It has fields Tid (Primary key), SellerId, BuyerId, StockId (Foreign Keys), Price and Date.
    {
        [Key]
        public  int Tid { get; set; }
        public int SellOrderId { get; set; }
        public int BuyOrderId { get; set; }
        public  int SellerId { get; set; }
        public  int BuyerId { get; set; }
        public  int StockId { get; set; }
        public  int StockNum { get; set; }
        public  double Price { get; set; }
        public  DateTime Date { get; set; } = DateTime.Now.ToUniversalTime();

        //[ForeignKey("SellerId")]
        //public User Seller { get; set; }
        
        //[ForeignKey("BuyerId")]
        //public User Buyer { get; set; }
        
        //[ForeignKey("StockId")]
        //public Stock Stock { get; set; }
        
        public Transaction()
        {
            
        }

        public Transaction(int tid, int sellOrderId, int buyOrderId, int sellerId, int buyerId, int stockId, int stockNum, double price)
        {
            Tid = tid;
            SellOrderId = sellOrderId;
            BuyOrderId = buyOrderId;
            SellerId = sellerId;
            BuyerId = buyerId;
            StockId = stockId;
            StockNum = stockNum;
            Price = price;
        }

        public override string ToString()
        {
            return $"Tid: {Tid}, SellOrderId: {SellOrderId}, BuyOrderId: {BuyOrderId}, SellerId: {SellerId}, BuyerId: {BuyerId}, StockId: {StockId}, StockNum: {StockNum}, Price: {Price}, Date: {Date}";
        }
    }
}