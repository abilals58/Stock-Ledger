using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Npgsql;

namespace Ledger.Ledger.Web.Repositories
{
    public interface IBuyOrderRepository
    {
        Task<IEnumerable<BuyOrder>> GetAllBuyOrdersAsync();
        Task<BuyOrder> GetBuyOrderByIdAsync(int id);
        Task<BuyOrder> AddBuyOrderAsync(BuyOrder buyOrder);
        Task<BuyOrder> UpdateByOrderAsync(int id, BuyOrder newbuyOrder);
        Task<BuyOrder> DeleteBuyOrderAsync(int id);
        Task UpdateBidSize(int id, int size); //decrement the bidSize by given size
        Task LogicalDelete(int id); // change the status of buyOrder false (deleted)
        Task<BuyOrder> GetMatchedBuyOrder(SellOrder sellOrder); // returns matched buyOrderIds with given sellOrderId
        Task<IEnumerable<int>> GetLatestBuyOrderIds();
        Task<BuyOrder> FindAndUpdateStatus(int buyOrderId, OrderStatus newOrderStatus);
        Task<IEnumerable<BuyOrder>> ChangeStatusActiveOnTheBeginningOfDay();
        Task ChangeStatusToNotCompletedAndDeleted();
        Task ChangeStatusToPartialyCompletedAndDeleted();
    }
    
    public class BuyOrderRepository : IBuyOrderRepository // BuyOrder service coresponds to data tier and handes database operations 
    {
        private readonly DbSet<BuyOrder> _dbBuyOrder;

        public BuyOrderRepository(IDbContext dbContext)
        {
            _dbBuyOrder = dbContext.BuyOrders;
        }

        public async Task<IEnumerable<BuyOrder>> GetAllBuyOrdersAsync() // returns al buyorders
        {
            return await _dbBuyOrder.ToListAsync();
        }

        public async Task<BuyOrder> GetBuyOrderByIdAsync(int id) // returns a buyorder by id
        {
            return await _dbBuyOrder.FindAsync(id);

        }

        public async Task<BuyOrder> AddBuyOrderAsync(BuyOrder buyOrder) // adds a buyorder to the database
        {
            await _dbBuyOrder.AddAsync(buyOrder);
            return buyOrder;
        }

        public async Task<BuyOrder> UpdateByOrderAsync(int id, BuyOrder newbuyOrder) // updates a buyorder an returns it, returns null if there is no match
        {
            var buyOrder = await _dbBuyOrder.FindAsync(id);
            if (buyOrder == null) return null;

            buyOrder.UserId = newbuyOrder.UserId;
            buyOrder.StockId = newbuyOrder.StockId;
            buyOrder.BidPrice = newbuyOrder.BidSize;
            buyOrder.BidSize = newbuyOrder.BidSize;
            buyOrder.StartDate = newbuyOrder.StartDate;
            return buyOrder;
        }

        public async Task<BuyOrder> DeleteBuyOrderAsync(int id) // deletes a buyorder and returns it, returns null if there is no match
        {
            var buyOrder = await _dbBuyOrder.FindAsync(id);
            if (buyOrder == null) return null;
            _dbBuyOrder.Remove(buyOrder);
            return buyOrder;
        }

        public async Task UpdateBidSize(int id, int size)
        {
            var buyOrder = await _dbBuyOrder.FindAsync(id);
            buyOrder.CurrentBidSize = buyOrder.CurrentBidSize - size;
        }
        
        public async Task LogicalDelete(int id) //changes the status to deleted (no)
        {
            var buyOrder = await _dbBuyOrder.FindAsync(id);
            buyOrder.Status = OrderStatus.CompletedAndDeleted;
        }

        public async Task<BuyOrder> GetMatchedBuyOrder(SellOrder sellOrder)
        {
            //update status of first matched buyOrder // return nullif no match
            
            //retrieve id of first match or null
            //var buyOrder = await _dbBuyOrder.FromSqlInterpolated($"UPDATE \"BuyOrders\" SET \"Status\" = 4 WHERE \"Status\" < 3 AND \"StockId\" = {sellOrder.StockId} AND \"BidPrice\" = {sellOrder.AskPrice} RETURNING * FOR UPDATE").FirstOrDefaultAsync();
            //var buyOrder = await _dbBuyOrder.FromSqlInterpolated($"SELECT * FROM \"BuyOrders\" WHERE \"Status\" < 3 AND \"StockId\" = {sellOrder.StockId} AND \"BidPrice\" = {sellOrder.AskPrice}")
                //.OrderBy(b => b.BuyOrderId).FirstOrDefaultAsync();

                var buyOrder = await _dbBuyOrder
                    .Where(b =>
                        (b.Status == OrderStatus.Active || b.Status == OrderStatus.PartiallyCompletedAndActive) &&
                        b.StockId == sellOrder.StockId && b.BidPrice == sellOrder.AskPrice).OrderBy(b => b.BuyOrderId)
                    .FirstOrDefaultAsync();
            if (buyOrder == null)
            {
                return null;
            }
            //update the status of buyOrder
            buyOrder.Status = OrderStatus.IsMatched;
            return buyOrder;
            
            /*Where(b =>
                 (b.Status == OrderStatus.Active || b.Status == OrderStatus.PartiallyCompletedAndActive)
                                                    && b.StockId == sellOrder.StockId &&
                                                    b.BidPrice == sellOrder.AskPrice)
            .Select(b => b.BuyOrderId)
            .ToListAsync();/*

            return null;

            /*if (buyOrders.Count == 0) // if there is no match, return null
            {
                return null;
            }

            var matchedBuyOrders = new List<int>();
            var totalSize = 0;
            //update status of matched buyOrders
            foreach (var buyOrderId in buyOrders)
            {
                var buyOrder = await _dbBuyOrder.FindAsync(buyOrderId);
                buyOrder.Status = OrderStatus.IsMatched;
                matchedBuyOrders.Add(buyOrderId);
                totalSize = totalSize + buyOrder.CurrentBidSize;
                if (totalSize >= sellOrder.CurrentAskSize)
                {
                    break;
                }
            }

            return matchedBuyOrders;*/
        }

        public async Task<IEnumerable<int>> GetLatestBuyOrderIds()
        {
            return await _dbBuyOrder.Where(b => b.Status == OrderStatus.IsMatched).OrderBy(b => b.BuyOrderId).Select(b=>b.BuyOrderId)
                .ToListAsync();
        }

        public async Task<BuyOrder> FindAndUpdateStatus(int buyOrderId, OrderStatus newOrderStatus)
        {
            var buyOrder = await _dbBuyOrder.FindAsync(buyOrderId);
            if (buyOrder == null)
            {
                return null;
            }

            buyOrder.Status = newOrderStatus;
            return buyOrder;
        }

        public async Task<IEnumerable<BuyOrder>> ChangeStatusActiveOnTheBeginningOfDay()
        {
            return await _dbBuyOrder
                .FromSqlRaw("UPDATE \"BuyOrders\"\nSET \"Status\" = 1\nWHERE \"Status\" = 3\nRETURNING *")
                .ToListAsync();
        }

        public async Task ChangeStatusToNotCompletedAndDeleted()
        {
            await _dbBuyOrder.FromSqlRaw("UPDATE \"BuyOrders\"\nSET \"Status\" = 8\nWHERE \"Status\" = 1\nRETURNING *").ToListAsync();
        }

        public async Task ChangeStatusToPartialyCompletedAndDeleted()
        {
            await _dbBuyOrder.FromSqlRaw("UPDATE \"BuyOrders\"\nSET \"Status\" = 7\nWHERE \"Status\" = 2\nRETURNING *").ToListAsync();
        }
    }
}