using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Ledger.Ledger.Web.Models
{
    public class BuyOrder    // This class refers to the object which stores relevant information about a buy order. It has BuyOrderId (primary key),
                            // UserId and StockId (Foreign Keys), BidPrice, BidSize, DateCreated fields.
    {
        [Key]
        public int BuyOrderId { get; set; } // randomly generated integer
        public int UserId { get; set; }
        public  int StockId { get; set; }
        public  double BidPrice { get; set; }
        public  int BidSize { get; set; }
        
        public int CurrentBidSize { get; set; } //current available bidSize if it is 0 then buyOrder is deleted
        public  DateTime StartDate { get; set; } = DateTime.Now.ToUniversalTime(); // buy order is active from startdate to end of the day
        public OrderStatus Status { get; set; } = OrderStatus.Active; // showing status of the buyOrder
        
        //[ForeignKey("UserId")]
        //public User User { get; set; } //navigation property
        
        //[ForeignKey("StockId")]
        //public Stock Stock { get; set; }
        
        public BuyOrder()
        {
            
        }

        public BuyOrder(int buyOrderId, int userId, int stockId, double bidPrice, int bidSize)
        {
            BuyOrderId = buyOrderId;
            UserId = userId;
            StockId = stockId;
            BidPrice = bidPrice;
            BidSize = bidSize;
            CurrentBidSize = bidSize;
            //check whether in working hours or not 

            if (DateTime.Now.Hour > 9 && DateTime.Now.Hour < 18)
            {
                Status = OrderStatus.Active;
            }
            else
            {
                Status = OrderStatus.NotYetActive;
            }
        }

        public override string ToString()
        {
            return $"BuyOrderId: {BuyOrderId}, UserId: {UserId}, StockId: {StockId}, BidPrice: {BidPrice}, BidSize: {BidSize}, " +

                   $"StartDate: {StartDate}";
        }
    }
}