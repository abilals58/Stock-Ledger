using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.UnitOfWork;

namespace Ledger.Ledger.Web.Services
{
    public interface ISellOrderMatchService
    {
        Task<IEnumerable<SellOrderMatch>> GetAllSellOrdersAsync();
        Task<SellOrderMatch> GetASellOrderMatchAsync(int sellOrderId, int buyOrderId);
        Task<SellOrderMatch> AddSellOrderMatchAsync(int sellOrderId, int buyOrderId);
        Task<IEnumerable<int>> GetMatchedBuyOrders(int sellOrderId);
    }
    
    
    public class SellOrderMatchService :ISellOrderMatchService
    {
        private readonly ISellOrderMatchRepository _sellOrderMatchRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SellOrderMatchService(ISellOrderMatchRepository sellOrderMatchRepository, IUnitOfWork unitOfWork)
        {
            _sellOrderMatchRepository = sellOrderMatchRepository;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<IEnumerable<SellOrderMatch>> GetAllSellOrdersAsync()
        {
            return await _sellOrderMatchRepository.GetAllSellOrdersAsync();
        }

        public async Task<SellOrderMatch> GetASellOrderMatchAsync(int sellOrderId, int buyOrderId)
        {
            return await _sellOrderMatchRepository.GetASellOrderMatchAsync(sellOrderId, buyOrderId);
        }

        public async Task<SellOrderMatch> AddSellOrderMatchAsync(int sellOrderId, int buyOrderId)
        {
            var sellOrderMatch =  await _sellOrderMatchRepository.AddSellOrderMatchAsync(sellOrderId, buyOrderId);
            await _unitOfWork.SaveChangesAsync();
            return sellOrderMatch;
        }

        public async Task<IEnumerable<int>> GetMatchedBuyOrders(int sellOrderId)
        {
            return await _sellOrderMatchRepository.GetMatchedBuyOrders(sellOrderId);
        }
    }
}