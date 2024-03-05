using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Ledger.Ledger.Web.Models
{
    public class DailyStock
    {
        [Key]
        public DateTime Date { get; set; }
        [Key]
        public int StockId { get; set; }
        public Double StockValue{ get; set; }
        
        
        public DailyStock()
        {
            
        }

        public DailyStock(DateTime date, int stockId, double stockValue)
        {
            Date = date.ToUniversalTime();
            StockId = stockId;
            StockValue = stockValue;
        }
        public override string ToString()
        {
            return $"Date: {Date}, StockId: {StockId}, StockValue: {StockValue}";
        }
        
    }
}