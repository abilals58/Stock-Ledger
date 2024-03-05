using System.Threading.Tasks;
using Ledger.Ledger.Web.Services;
using Quartz;

namespace Ledger.Ledger.Web.Jobs
{
    public class OpenSystemJob : IJob
    {
        private readonly ISellOrderService _sellOrderService;
        private readonly ISellOrderProcessService _sellOrderProcessService;
        private readonly IBuyOrderService _buyOrderService;
        private readonly IBuyOrderProcessService _buyOrderProcessService;

        public OpenSystemJob(ISellOrderService sellOrderService, ISellOrderProcessService sellOrderProcessService, IBuyOrderService buyOrderService, IBuyOrderProcessService buyOrderProcessService)
        {
            _sellOrderService = sellOrderService;
            _sellOrderProcessService = sellOrderProcessService;
            _buyOrderService = buyOrderService;
            _buyOrderProcessService = buyOrderProcessService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            //add sellOrders and buyOrders created in out of working hours to sellOrderJobs and BuyOrderJobs tables 
            
            //change status of all SellOrders from notyetactive to active
            var sellOrders = await _sellOrderService.ChangeStatusActiveOnTheBeginningOfDay();
            //add all sellOrderProcesses
            foreach (var sellOrder in sellOrders)
            {
                // add new sellOrderProcess
                await _sellOrderProcessService.AddSellOrderProcessBySellOrder(sellOrder);

            }
            //change status of all buyOrders from notyetactive to active 
            var buyOrders = await _buyOrderService.ChangeStatusActiveOnTheBeginningOfDay();
            //add all buyOrderProcesses
            foreach (var buyOrder in buyOrders)
            {
                await _buyOrderProcessService.AddBuyOrderProcessByBuyOrder(buyOrder);
            }
            
        }
    }
}