using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.UnitOfWork;

namespace Ledger.Ledger.Web.Services;

public interface IBuyOrderProcessService
{
    //Task<BuyOrderProcess> GetMatchedBuyOrderProcess(SellOrderProcess sellOrderProcess);
    Task AddBuyOrderProcessByBuyOrder(BuyOrder buyOrder);
    Task DeleteAllBuyOrderProcesses();
}

public class BuyOrderProcessService : IBuyOrderProcessService
{
    private readonly IBuyOrderProcessRepository _buyOrderProcessRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BuyOrderProcessService(IBuyOrderProcessRepository buyOrderProcessRepository,ISellOrderMatchRepository sellOrderMatchRepository, IUnitOfWork unitOfWork)
    {
        _buyOrderProcessRepository = buyOrderProcessRepository;
        _unitOfWork = unitOfWork;
    }
    
    /*public async Task<BuyOrderProcess> GetMatchedBuyOrderProcess(SellOrderProcess sellOrderProcess)
    {
        var buyOrderProcess = await _buyOrderProcessRepository.GetMatchedBuyOrderProcess(sellOrderProcess);
        await _unitOfWork.SaveChangesAsync();
        return buyOrderProcess;
    }*/
    public async Task AddBuyOrderProcessByBuyOrder(BuyOrder buyOrder)
    {
        await _buyOrderProcessRepository.AddBuyOrderProcess(new BuyOrderProcess(default, buyOrder.BuyOrderId,
            buyOrder.Status, buyOrder.StockId, buyOrder.BidPrice));
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAllBuyOrderProcesses()
    {
        await _buyOrderProcessRepository.DeleteAllBuyOrderProcesses();
        await _unitOfWork.SaveChangesAsync();
    }
}