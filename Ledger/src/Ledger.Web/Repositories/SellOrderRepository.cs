using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Ledger.Web.Repositories
{
    public interface ISellOrderRepository
    {
        Task<IEnumerable<SellOrder>> GetAllSellOrdersAsync();
        Task<SellOrder> GetSellOrderByIdAsync(int id);
        Task<SellOrder> AddSellOrderAsync(SellOrder sellOrder);
        Task<SellOrder> UpdateSellOrderAsync(int id, SellOrder newSellOrder);
        Task<SellOrder> DeleteSellOrderAsync(int id);
        Task UpdateAskSizeAsync(int id, int size);
        //Task<IEnumerable<int>> GetMatchedSellOrderIds(BuyOrder buyOrder);
        //Task<IEnumerable<int>> GetLatestSellOderIds();
        Task LogicalDelete(int id);

        Task<SellOrder> FindAndUpdateStatus(int sellOrderId, OrderStatus newStatus);

        Task<IEnumerable<SellOrder>> ChangeStatusActiveOnTheBeginningOfDay();

        Task ChangeStatusToNotCompletedAndDeleted(); // active => not completed deleted

        Task ChangeStatusToPartiallyCompletedDeleted(); // partially completed and active => partially completed and deleted
        //Task<IEnumerable<int>> MatchBuyOrdersAsync(int sellOrderId);
    }
    public class SellOrderRepository : ISellOrderRepository // SellOrder service corresponds to data tier and it handles database operations
    {
        private readonly DbSet<SellOrder> _dbSellOrder;

        public SellOrderRepository(IDbContext dbContext)
        {
            _dbSellOrder = dbContext.SellOrders;
        }
        
        public async Task<IEnumerable<SellOrder>> GetAllSellOrdersAsync() // returns all sellorders
        {
            return await _dbSellOrder.ToListAsync();
        }
        
        public async Task<SellOrder> GetSellOrderByIdAsync(int id) // returns a sellorder by id
        {
            return await _dbSellOrder.FindAsync(id);
            
        }
        
        public async Task<SellOrder> AddSellOrderAsync(SellOrder sellOrder) // adds a sellorder to the database
        {
            await _dbSellOrder.AddAsync(sellOrder);
            return sellOrder;
        }
        
        public async Task<SellOrder> UpdateSellOrderAsync(int id, SellOrder newSellOrder) // updates a sellorder and returns it, returns null if there is no match
        {
            var sellOrder = await _dbSellOrder.FindAsync(id);
            if (sellOrder == null) return null;
            sellOrder.UserId = newSellOrder.UserId;
            sellOrder.StockId = newSellOrder.StockId;
            sellOrder.AskPrice = newSellOrder.AskPrice;
            sellOrder.AskSize = newSellOrder.AskSize;
            sellOrder.StartDate = newSellOrder.StartDate;
            return sellOrder;
        }
        
        public async Task<SellOrder> DeleteSellOrderAsync(int id) // deletes a sellorder and returns it, returns null if there is no match
        {
            var sellOrder = await _dbSellOrder.FindAsync(id);
            if (sellOrder == null) return null;
            _dbSellOrder.Remove(sellOrder); //TODO: REMOVE BY ID 
            return sellOrder;
        }

        public async Task UpdateAskSizeAsync(int id, int size) //decrements the askSize by given size
        {
            var sellOrder = await _dbSellOrder.FindAsync(id);
            sellOrder.CurrentAskSize -= size;
        }

        /*public async Task<IEnumerable<int>> GetMatchedSellOrderIds(BuyOrder buyOrder)
        {
            // retrieve matched sellOrders
            var sellOrders = await _dbSellOrder.Where(s =>
                (s.Status == OrderStatus.Active || s.Status == OrderStatus.PartiallyCompletedAndActive) &&
                s.StockId == buyOrder.StockId && s.AskPrice == buyOrder.BidPrice).Select(s=>s.SellOrderId).ToListAsync();

            if (sellOrders.Count() == 0) //if there is no match, return null
            {
                return null;
            }

            var matchedSellOrders = new List<int>();
            var totalSize = 0;
            // change status of matched sellOrdes
            foreach (var sellOrderId in sellOrders)
            {
                var sellOrder = await _dbSellOrder.FindAsync(sellOrderId);
                sellOrder.Status = OrderStatus.IsMatched;
                matchedSellOrders.Add(sellOrderId);
                totalSize = totalSize + sellOrder.CurrentAskSize;
                if (totalSize >= buyOrder.CurrentBidSize )
                {
                    break;
                }
            }

            return matchedSellOrders;
        }*/

        /*public async Task<IEnumerable<int>> GetLatestSellOderIds()
        {
            return await _dbSellOrder.Where(s => s.Status == OrderStatus.Active || s.Status == OrderStatus.PartiallyCompletedAndActive).OrderBy(s=>s.SellOrderId).Select(s => s.SellOrderId)
                .ToListAsync();
        }*/
        public async Task LogicalDelete(int id) //changes the status to deleted (no)
        {
            var sellOrder = await _dbSellOrder.FindAsync(id);
            sellOrder.Status = OrderStatus.CompletedAndDeleted;
        }

        public async Task<SellOrder> FindAndUpdateStatus(int sellOrderId, OrderStatus newStatus)
        {
            var sellOrder = await _dbSellOrder.FindAsync(sellOrderId);
            if (sellOrder == null)
            {
                return null;
            }

            if (sellOrder.Status != newStatus)
            {
                sellOrder.Status = newStatus;
            }
            return sellOrder;
        }

        public async Task<IEnumerable<SellOrder>> ChangeStatusActiveOnTheBeginningOfDay()
        {
            return await _dbSellOrder.FromSqlRaw("UPDATE \"SellOrders\"\nSET \"Status\" = 1\nWHERE \"Status\" = 3\nRETURNING *").ToListAsync();
        }

        public async Task ChangeStatusToNotCompletedAndDeleted()
        {
            await _dbSellOrder.FromSqlRaw("UPDATE \"SellOrders\"\nSET \"Status\" = 8\nWHERE \"Status\" = 1\nRETURNING *").ToListAsync();
        }

        public async Task ChangeStatusToPartiallyCompletedDeleted()
        {
            await _dbSellOrder.FromSqlRaw("UPDATE \"SellOrders\"\nSET \"Status\" = 7\nWHERE \"Status\" = 2\nRETURNING *").ToListAsync();
        }
    }
}