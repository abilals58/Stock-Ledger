using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.UnitOfWork;

namespace Ledger.Ledger.Web.Services
{
    
    public interface IStocksOfUserService
    {
        Task<IEnumerable<StocksOfUser>> GetAllStocksOfUserAsync();
        Task<StocksOfUser> GetAStocksOfUserAsync(int userId, int stockId);
        Task<StocksOfUser> AddStocksOfUserAsync(StocksOfUser stocksOfUser);
        Task<StocksOfUser> UpdateStocksOfUserAsync(int id, StocksOfUser newStocksOfUser);
        Task<StocksOfUser> DeleteStocksOfUserAsync(int id);
    }
    public class StocksOfUserService : IStocksOfUserService
    {

        private readonly IStocksOfUserRepository _stocksOfUserRepository;
        private IUnitOfWork _unitOfWork;

        public StocksOfUserService(IStocksOfUserRepository stocksOfUserRepository, IUnitOfWork unitOfWork)
        {
            _stocksOfUserRepository = stocksOfUserRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<StocksOfUser>> GetAllStocksOfUserAsync()
        {
            return await _stocksOfUserRepository.GetAllStocksOfUserAsync();
        }

        public async Task<StocksOfUser> GetAStocksOfUserAsync(int userId, int stockId)
        {
            return await _stocksOfUserRepository.GetAStocksOfUserAsync(userId, stockId);        
        }

        public async Task<StocksOfUser> AddStocksOfUserAsync(StocksOfUser stocksOfUser)
        {
            await _stocksOfUserRepository.AddStocksOfUserAsync(stocksOfUser);
            await _unitOfWork.SaveChangesAsync();
            return stocksOfUser;
        }

        public async Task<StocksOfUser> UpdateStocksOfUserAsync(int id, StocksOfUser newStocksOfUser)
        {
            var stocksOfUser = await _stocksOfUserRepository.UpdateStocksOfUserAsync(id, newStocksOfUser);
            await _unitOfWork.SaveChangesAsync();
            return stocksOfUser;
        }

        public async Task<StocksOfUser> DeleteStocksOfUserAsync(int id)
        {
            var stocksOfUser = await _stocksOfUserRepository.DeleteStocksOfUserAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return stocksOfUser;
        }
    }
}