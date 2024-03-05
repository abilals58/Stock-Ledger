using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Ledger.Web.Repositories
{
    public interface IStockRepository
    {
        Task<IEnumerable<Stock>> GetAllStocksAsync();
        Task<Stock> GetStockByIdAsync(int id);
        Task<Stock> AddStockAsync(Stock stock);
        Task<Stock> UpdateStockAsync(int id, Stock newStock);
        Task<Stock> DeleteStockAsync(int id);
        Task<Stock> UpdateStockPriceAsync(int id, double currentPrice, double highestPrice, double lowestPrice);
    }
    public class StockRepository : IStockRepository
    {
        private readonly DbSet<Stock> _dbStock;

        public StockRepository(IDbContext dbContext) // Stock service corresponds to data access tier and handles database operations
        {
            _dbStock = dbContext.Stocks;
        }
        
        public async Task<IEnumerable<Stock>> GetAllStocksAsync() // returns all stocks
        {
            return await _dbStock.ToListAsync();
        }
        
        public async Task<Stock> GetStockByIdAsync(int id) // returns a stock by id
        {
            return await _dbStock.FindAsync(id);
            
        }
        
        public async Task<Stock> AddStockAsync(Stock stock)  // adds a stock to the database
        {
            await _dbStock.AddAsync(stock);
            return stock;
        }
        
        public async Task<Stock> UpdateStockAsync(int id, Stock newStock) // updates the stock and returns that stock, returns null if there is no match 
        {
            var stock = await _dbStock.FindAsync(id);
            if (stock == null) return null;
            stock.StockName = newStock.StockName;
            stock.OpenDate = newStock.OpenDate;
            stock.InitialStock = newStock.InitialStock;
            stock.InitialPrice = newStock.InitialPrice;
            stock.CurrentStock = newStock.CurrentStock;
            stock.CurrentPrice = newStock.CurrentPrice;
            stock.HighestPrice = newStock.HighestPrice;
            stock.LowestPrice = newStock.LowestPrice;
            stock.Status = newStock.Status;
            return stock;
        }
        
        public async Task<Stock> DeleteStockAsync(int id) // deletes a stock and returns that stock, returns null if there is no match
        {
            var stock = await _dbStock.FindAsync(id);
            if (stock == null) return null;
            _dbStock.Remove(stock);
            return stock;
        }
        public async Task<Stock> UpdateStockPriceAsync(int id, double currentPrice, double highestPrice, double lowestPrice) // updates the stock and returns that stock, returns null if there is no match 
        {
            var stock = await _dbStock.FindAsync(id);
            if (stock == null) return null;
            
            stock.CurrentPrice = currentPrice;
            stock.HighestPrice = highestPrice;
            stock.LowestPrice = lowestPrice;
            return stock;
        }
    }
}
