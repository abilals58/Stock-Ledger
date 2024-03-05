using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Ledger.Web.Repositories
{
    public interface ISellOrderMatchRepository
    {
        Task<IEnumerable<SellOrderMatch>> GetAllSellOrdersAsync();
        Task<SellOrderMatch> GetASellOrderMatchAsync(int sellOrderId, int buyOrderId);
        Task<SellOrderMatch> AddSellOrderMatchAsync(int sellOrderId, int buyOrderId);
        Task<IEnumerable<int>> GetMatchedBuyOrders(int sellOrderId);
    }
    
    
    public class SellOrderMatchRepository :ISellOrderMatchRepository
    {

        private readonly DbSet<SellOrderMatch> _dbSellOrderMatch;

        public SellOrderMatchRepository(IDbContext dbContext)
        {
            _dbSellOrderMatch = dbContext.SellOrderMatches;
        }


        public async Task<IEnumerable<SellOrderMatch>> GetAllSellOrdersAsync()
        {
            return await _dbSellOrderMatch.ToListAsync();
        }

        public async Task<SellOrderMatch> GetASellOrderMatchAsync(int sellOrderId, int buyOrderId)
        {
            return await _dbSellOrderMatch.FindAsync(sellOrderId, buyOrderId);
        }

        public async Task<SellOrderMatch> AddSellOrderMatchAsync(int sellOrderId, int buyOrderId)
        {
            var sellOrderMatch = new SellOrderMatch(sellOrderId, buyOrderId);
            await _dbSellOrderMatch.AddAsync(sellOrderMatch);
            return sellOrderMatch;

        }

        public async Task<IEnumerable<int>> GetMatchedBuyOrders(int sellOrderId) // return all sellOrderId, BuyOrderId pairs for a sellOrder
        {
            return await _dbSellOrderMatch.Where(s => s.SellOrderId == sellOrderId).Select(s=>s.BuyOrderId).ToListAsync();
        }
    }
}