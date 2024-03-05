using System.Linq;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Ledger.Web.Repositories;

public interface IBuyOrderProcessRepository
{
    Task AddBuyOrderProcess(BuyOrderProcess buyOrderProcess);
    Task<BuyOrderProcess> FindMatchedBuyOrderAndUpdateStatusIsMatched(SellOrderProcess sellOrderProcess);
    Task<BuyOrderProcess> FindAndUpdateStatus(int buyOrderProcessId, OrderStatus newStatus);
    Task DeleteByOrderProcessByBuyOrderId(int buyOrderId);
    Task DeleteAllBuyOrderProcesses();
}


public class BuyOrderProcessRepository :IBuyOrderProcessRepository
{
    private readonly DbSet<BuyOrderProcess> _dbBuyOrderProcess;

    public BuyOrderProcessRepository(IDbContext dbContext)
    {
        _dbBuyOrderProcess = dbContext.BuyOrderJobs;
    }

    public async Task AddBuyOrderProcess(BuyOrderProcess buyOrderProcess)
    {
        await _dbBuyOrderProcess.AddAsync(buyOrderProcess);
    }

    public async Task<BuyOrderProcess> FindMatchedBuyOrderAndUpdateStatusIsMatched(SellOrderProcess sellOrderProcess)
    {
        // find matched buyOrderProcess set its status isMatched and return it (implemented in database level)
        var buyOrderProcessList = await _dbBuyOrderProcess.FromSqlInterpolated(
            $"UPDATE \"BuyOrderJobs\"\nSET \"Status\" = 5\nWHERE \"BuyOrderId\" = (\n    SELECT MIN(\"BuyOrderId\")\n    FROM \"BuyOrderJobs\"\n    WHERE \"Status\" IN (1, 2) AND \"StockId\" = {sellOrderProcess.StockId} AND \"BidPrice\" = {sellOrderProcess.AskPrice}\n    LIMIT 1\n)\nRETURNING *").ToListAsync();
        if (!buyOrderProcessList.Any())
        {
            return null;
        }
        return buyOrderProcessList[0];
    }

    public async Task<BuyOrderProcess> FindAndUpdateStatus(int buyOrderProcessId, OrderStatus newStatus)
    {
        var buyOrderProcess = await _dbBuyOrderProcess.FindAsync(buyOrderProcessId);
        if (buyOrderProcess == null)
        {
            return null;
        }

        if (buyOrderProcess.Status != newStatus)
        {
            buyOrderProcess.Status = newStatus;
        }

        return buyOrderProcess;
    }

    public async Task DeleteByOrderProcessByBuyOrderId(int buyOrderId)
    {
        var buyOrderProcess = await _dbBuyOrderProcess.Where(b => b.BuyOrderId == buyOrderId).FirstOrDefaultAsync();
        _dbBuyOrderProcess.Remove(buyOrderProcess);
    }

    public async Task DeleteAllBuyOrderProcesses()
    {
        var buyOrderProcesses = await _dbBuyOrderProcess.ToListAsync();
        _dbBuyOrderProcess.RemoveRange(buyOrderProcesses);
    }
}