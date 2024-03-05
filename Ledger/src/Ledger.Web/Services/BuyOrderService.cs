using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.UnitOfWork;
using Microsoft.VisualBasic;

namespace Ledger.Ledger.Web.Services
{
    public interface IBuyOrderService
    {
        Task<IEnumerable<BuyOrder>> GetAllBuyOrdersAsync();
        Task<BuyOrder> GetBuyOrderByIdAsync(int id);
        Task<BuyOrder> AddBuyOrderAsync(BuyOrder buyOrder);
        Task<BuyOrder> UpdateByOrderAsync(int id, BuyOrder newbuyOrder);
        Task<BuyOrder> DeleteBuyOrderAsync(int id);
        Task<IEnumerable<int>> GetLatestBuyOrderIds();
        Task<IEnumerable<BuyOrder>> ChangeStatusActiveOnTheBeginningOfDay();
        Task ChangeStatusToNotCompletedAndDeleted();
        Task ChangeStatusToPartialyCompletedAndDeleted();

        Task<IEnumerable<Transaction>> GetTransactionsOfABuyOrder(int buyOrderId);
    }
    public class BuyOrderService :IBuyOrderService
    {
        private readonly IBuyOrderRepository _buyOrderRepository;
        private readonly ISellOrderRepository _sellOrderRepository;
        private readonly IStocksOfUserRepository _stocksOfUserRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IBuyOrderProcessRepository _buyOrderProcessRepository;
        private IUnitOfWork _unitOfWork;

        public BuyOrderService(IBuyOrderRepository buyOrderRepository, ISellOrderRepository sellOrderRepository,IStocksOfUserRepository stocksOfUserRepository, ITransactionRepository transactionRepository,IUserRepository userRepository, IStockRepository stockRepository,IBuyOrderProcessRepository buyOrderProcessRepository, IUnitOfWork unitOfWork)
        {
            _buyOrderRepository = buyOrderRepository;
            _sellOrderRepository = sellOrderRepository;
            _stocksOfUserRepository = stocksOfUserRepository;
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _stockRepository = stockRepository;
            _buyOrderProcessRepository = buyOrderProcessRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<BuyOrder>> GetAllBuyOrdersAsync()
        {
            return await _buyOrderRepository.GetAllBuyOrdersAsync();
        }

        public async Task<BuyOrder> GetBuyOrderByIdAsync(int id)
        {
            return await _buyOrderRepository.GetBuyOrderByIdAsync(id);        }

        public async Task<BuyOrder> AddBuyOrderAsync(BuyOrder buyOrder)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                await _buyOrderRepository.AddBuyOrderAsync(buyOrder);
                await _unitOfWork.SaveChangesAsync();
                // add buy order process
                await this.AddBuyOrderProcessByBuyOrder(buyOrder);
                await _unitOfWork.CommitAsync();
                return buyOrder;
            }
            catch (Exception e)
            {
                await _unitOfWork.RollBackAsync();
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<BuyOrder> UpdateByOrderAsync(int id, BuyOrder newbuyOrder)
        {
            var buyOrder = await _buyOrderRepository.UpdateByOrderAsync(id, newbuyOrder);
            await _unitOfWork.SaveChangesAsync();
            return buyOrder;
        }

        public async Task<BuyOrder> DeleteBuyOrderAsync(int id)
        {
            var buyOrder = await _buyOrderRepository.DeleteBuyOrderAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return buyOrder;
        }
        public async Task<IEnumerable<int>> GetLatestBuyOrderIds()
        {
            return await _buyOrderRepository.GetLatestBuyOrderIds();
        }

        public async Task<IEnumerable<BuyOrder>> ChangeStatusActiveOnTheBeginningOfDay()
        {
            return await _buyOrderRepository.ChangeStatusActiveOnTheBeginningOfDay();
        }

        public async Task ChangeStatusToNotCompletedAndDeleted()
        {
            await _buyOrderRepository.ChangeStatusToNotCompletedAndDeleted();
        }

        public async Task ChangeStatusToPartialyCompletedAndDeleted()
        {
            await _buyOrderRepository.ChangeStatusToPartialyCompletedAndDeleted();
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsOfABuyOrder(int buyOrderId)
        {
            return await _transactionRepository.GetTransactionsOfABuyOrder(buyOrderId);
        }

        private async Task AddBuyOrderProcessByBuyOrder(BuyOrder buyOrder)
        {
            // if we are in working hours add buyOrderProcess
            if (buyOrder.Status != OrderStatus.NotYetActive)
            {
                await _buyOrderProcessRepository.AddBuyOrderProcess(new BuyOrderProcess(default, buyOrder.BuyOrderId,
                    buyOrder.Status, buyOrder.StockId, buyOrder.BidPrice));
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}