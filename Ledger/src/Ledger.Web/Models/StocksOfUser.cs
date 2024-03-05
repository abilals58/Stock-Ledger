using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ledger.Ledger.Web.Models
{
    public class StocksOfUser // This class refers to the raws of a table in the database which relates a user with its owning stocks. StocksOfUser table contains
                              // related information about a user's stocks; therefore, it has fields StocksOfUserId (primary key), UserId and StockId (Foreign Keys) and NumOfStocks.
    {
        public  int UserId { get; set; }
        public  int StockId { get; set; }
        public  int NumOfStocks { get; set; }
        
        //[ForeignKey("UserId")]
        //public User User { get; set; } //navigation property
        
        //[ForeignKey("StockId")]
        //public Stock Stock { get; set; }

        public  StocksOfUser()
        {
            
        }

        public StocksOfUser(int userId, int stockId, int numOfStocks)
        {
            UserId = userId;
            StockId = stockId;
            NumOfStocks = numOfStocks;
        }
        
        public override string ToString()
        {
            return $"UserId: {UserId}, StockId: {StockId}, NumOfStocks: {NumOfStocks}";
        }
    }
}