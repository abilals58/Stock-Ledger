using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.Services;
using Ledger.Ledger.Web.UnitOfWork;
using Quartz;

namespace Ledger.Ledger.Web.Jobs
{
    public class CloseSystemJob : IJob
    {
        private readonly IDailyStockService _dailyStockService;
        private readonly ISellOrderService _sellOrderService;
        private readonly ISellOrderProcessService _sellOrderProcessService;
        private readonly IBuyOrderService _buyOrderService;
        private readonly IBuyOrderProcessService _buyOrderProcessService;

        public CloseSystemJob(IDailyStockService dailyStockService, ISellOrderService sellOrderService, ISellOrderProcessService sellOrderProcessService, IBuyOrderService buyOrderService, IBuyOrderProcessService buyOrderProcessService)
        {
            _dailyStockService = dailyStockService;
            _sellOrderService = sellOrderService;
            _sellOrderProcessService = sellOrderProcessService;
            _buyOrderService = buyOrderService;
            _buyOrderProcessService = buyOrderProcessService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            //record the ending values of all stocks
            
            await _dailyStockService.RecordAllDailyStocksAsync(); 
            
            //in sellOrder and buyOrder Tables:
            // if status is partially completed and active --> partially completed and deleted
            // if status is active --> not completed and deleted
            //sellOrders status update
            await _sellOrderService.ChangeStatusToNotCompletedAndDeleted();
            await _sellOrderService.ChangeStatusToPartiallyCompletedDeleted();
            // buyOrders status update
            await _buyOrderService.ChangeStatusToNotCompletedAndDeleted();
            await _buyOrderService.ChangeStatusToPartialyCompletedAndDeleted();
            
            //in SellOrderJobs and BuyOrderJobs :
            //completely delete tables
            await _sellOrderProcessService.DeleteAllSellOrderProcesses();
            await _buyOrderProcessService.DeleteAllBuyOrderProcesses();
        }
    }
}