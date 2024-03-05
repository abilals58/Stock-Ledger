using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.UnitOfWork;

namespace Ledger.Ledger.Web.Services;

public interface ISellOrderProcessService
{
    Task<SellOrderProcess> FindNextProcessAndSetStatusProcessing();
    Task<SellOrderProcess> UpdateOrderNum(int sellOrderProcessId);
    Task<SellOrderProcess> SetStatusActiveBySellOrderProcessId(int sellOrderProcessId);
    Task AddSellOrderProcessBySellOrder(SellOrder sellOrder);
    Task DeleteAllSellOrderProcesses();

}

public class SellOrderProcessService : ISellOrderProcessService
{
    private ISellOrderProcessRepository _sellOrderProcessRepository;
    private ISellOrderRepository _sellOrderRepository;
    private IUnitOfWork _unitOfWork;

    public SellOrderProcessService(ISellOrderProcessRepository sellOrderProcessRepository, ISellOrderRepository sellOrderRepository, IUnitOfWork unitOfWork)
    {
        _sellOrderProcessRepository = sellOrderProcessRepository;
        _sellOrderRepository = sellOrderRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<SellOrderProcess> FindNextProcessAndSetStatusProcessing()
    {
        return await _sellOrderProcessRepository.FindNextProcessAndSetStatusProcessing();
    }

    public async Task<SellOrderProcess> UpdateOrderNum(int sellOrderProcessId)
    {
        var sellOrderProcess = await _sellOrderProcessRepository.UpdateOrderNum(sellOrderProcessId);
        await _unitOfWork.SaveChangesAsync();
        return sellOrderProcess;
    }

    public async Task<SellOrderProcess> SetStatusActiveBySellOrderProcessId(int sellOrderProcessId)
    {
        var sellOrderProcess = await _sellOrderProcessRepository.FindAndUpdateStatus(sellOrderProcessId, OrderStatus.Active);
        await _unitOfWork.SaveChangesAsync();
        return sellOrderProcess;
    }
    
    public async Task AddSellOrderProcessBySellOrder(SellOrder sellOrder) 
    {
        // add sellOrderProcess
        await _sellOrderProcessRepository.AddSellOrderProcess(new SellOrderProcess(default, sellOrder.SellOrderId,
            sellOrder.Status, sellOrder.StockId, sellOrder.AskPrice));
        await _unitOfWork.SaveChangesAsync();
            
    }

    public async Task DeleteAllSellOrderProcesses()
    {
        await _sellOrderProcessRepository.DeleteAllSellOrderProcesses();
        await _unitOfWork.SaveChangesAsync();
    }
}