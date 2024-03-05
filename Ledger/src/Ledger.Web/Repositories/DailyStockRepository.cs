using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Ledger.Web.Repositories
{

    public interface IDailyStockRepository
    {
        Task<IEnumerable<DailyStock>> GetAllDailyStocksAsync();
        Task<DailyStock> GetDailyStockByDateAsync(int id, DateTime date);
        Task<DailyStock> AddDailyStockAsync(DailyStock dailystock);
        Task<DailyStock> UpdateDailyStockAsync(int id, DateTime date, DailyStock newdailystock);
        Task<DailyStock> DeleteDailyStockAsync(int id, DateTime date);
        Task<IEnumerable<DailyStock>> GetDailyStocksOfAStock(int id);
    }
    
    public class DailyStockRepository : IDailyStockRepository
    {

        private readonly DbSet<DailyStock> _dbDailyStock;
        public DailyStockRepository(IDbContext dbContext)
        {
            _dbDailyStock = dbContext.DailyStocks;
        }
        
        public async Task<IEnumerable<DailyStock>> GetAllDailyStocksAsync()
        {
            return await _dbDailyStock.ToListAsync();
        }

        public async Task<DailyStock> GetDailyStockByDateAsync(int id, DateTime date)
        {
            return await _dbDailyStock.FindAsync(id, date);
        }

        public async Task<DailyStock> AddDailyStockAsync(DailyStock dailystock)
        {
            await _dbDailyStock.AddAsync(dailystock);
            return dailystock;
        }
        
        public async Task<DailyStock> UpdateDailyStockAsync(int id, DateTime date, DailyStock newdailystock)
        {
            var dailystock = await  _dbDailyStock.FindAsync(id, date);
            if (dailystock == null) return null;
            
            dailystock.StockValue = newdailystock.StockValue;
            return dailystock;
        }

        public async Task<DailyStock> DeleteDailyStockAsync(int id, DateTime date)
        {
            var dailystock = await  _dbDailyStock.FindAsync(id, date );
            if (dailystock == null) return null;

            _dbDailyStock.Remove(dailystock);
            return dailystock;
        }

        public async Task<IEnumerable<DailyStock>> GetDailyStocksOfAStock(int id)
        {
            return await _dbDailyStock.Where(ds => ds.StockId == id).ToListAsync();
        }
    }
}