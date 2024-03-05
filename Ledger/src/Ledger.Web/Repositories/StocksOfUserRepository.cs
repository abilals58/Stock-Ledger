using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Ledger.Web.Repositories
{

    public interface IStocksOfUserRepository
    {
        Task<IEnumerable<StocksOfUser>> GetAllStocksOfUserAsync();
        Task<StocksOfUser> GetAStocksOfUserAsync(int userId, int stockId);
        Task<StocksOfUser> AddStocksOfUserAsync(StocksOfUser stocksOfUser);
        Task<StocksOfUser> UpdateStocksOfUserAsync(int id, StocksOfUser newStocksOfUser);
        Task<StocksOfUser> DeleteStocksOfUserAsync(int id);
        Task<StocksOfUser> UpdateStockInfo(int userId, int stockId, char sign, int size); //update stock size information according to the sign and size
    }
    public class StocksOfUserRepository : IStocksOfUserRepository// StocksOfUser service corresponds to data access tier and handles database operations
    {
        private readonly DbSet<StocksOfUser> _dbStocksOfUser;

        public StocksOfUserRepository(IDbContext dbContext)
        {
            _dbStocksOfUser = dbContext.StocksOfUser;
        }
        
        public async Task<IEnumerable<StocksOfUser>> GetAllStocksOfUserAsync() // returns all stocksofusers
        {
            return await _dbStocksOfUser.ToListAsync();
        }
        
        public async Task<StocksOfUser> GetAStocksOfUserAsync(int userId, int stockId) // returns a stocksofuser by id
        {
            return await _dbStocksOfUser.FindAsync(userId, stockId);
           
        }

        public async Task<StocksOfUser> AddStocksOfUserAsync( StocksOfUser stocksOfUser) // add a stocksofuser
        {
            await _dbStocksOfUser.AddAsync(stocksOfUser);
            return stocksOfUser;
        }
        
        public async Task<StocksOfUser> UpdateStocksOfUserAsync(int id, StocksOfUser newStocksOfUser) // update a stocksofuser and return it, returns null if there is no match
        {
            var stocksOfUser = await _dbStocksOfUser.FindAsync(id);
            if (stocksOfUser == null) return null;
            stocksOfUser.UserId = newStocksOfUser.UserId;
            stocksOfUser.StockId = newStocksOfUser.StockId;
            stocksOfUser.NumOfStocks = newStocksOfUser.NumOfStocks;
            return stocksOfUser;

        }
        
        public async Task<StocksOfUser> DeleteStocksOfUserAsync(int id) // delete a stocksofuser and return it, return null if there is no match
        {
            var stocksOfUser = await _dbStocksOfUser.FindAsync(id);
            if (stocksOfUser == null) return null;
            _dbStocksOfUser.Remove(stocksOfUser);
            return stocksOfUser;
        }

        public async Task<StocksOfUser> UpdateStockInfo(int userId, int stockId, char sign, int size)
        {
            var stocksOfUser = await _dbStocksOfUser.FindAsync(userId, stockId);
            if (stocksOfUser == null) return null;
            //update size information accordingly

            if (sign == '+')
            {
                stocksOfUser.NumOfStocks = stocksOfUser.NumOfStocks + size;
            }
            else
            {
                stocksOfUser.NumOfStocks = stocksOfUser.NumOfStocks - size;
            }

            return stocksOfUser;

        }
    }
}