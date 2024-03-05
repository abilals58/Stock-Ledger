using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Jobs;
using Ledger.Ledger.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Ledger.Web.Repositories;

public interface ISellOrderProcessRepository
{
    Task AddSellOrderProcess(SellOrderProcess sellOrderProcess);
    Task<SellOrderProcess> GetSellOrderProcessById(int sellOrderProcessId);
    Task<SellOrderProcess> FindNextProcessAndSetStatusProcessing();
    Task<SellOrderProcess> FindAndUpdateStatus(int sellOrderProcessId, OrderStatus newStatus);

    Task<SellOrderProcess> UpdateOrderNum(int sellOrderProcessId);

    Task DeleteSellOrderProcessBySellOrderId(int sellOrderId);
    Task DeleteAllSellOrderProcesses();
}

public class SellOrderProcessRepository : ISellOrderProcessRepository
{
    private readonly DbSet<SellOrderProcess> _dbSellOrderProcess;

    public SellOrderProcessRepository(IDbContext dbContext)
    {
        _dbSellOrderProcess = dbContext.SellOrderJobs;
    }

    public async Task AddSellOrderProcess(SellOrderProcess sellOrderProcess)
    {
        await _dbSellOrderProcess.AddAsync(sellOrderProcess);
    }

    public async Task<SellOrderProcess> GetSellOrderProcessById(int sellOrderProcessId)
    {
        return await _dbSellOrderProcess.Where(s => s.SellOrderProcessId == sellOrderProcessId).FirstOrDefaultAsync();
    }

    public async Task<SellOrderProcess> FindNextProcessAndSetStatusProcessing()
    {
        //return the sellOrderProcess according to FIFO order (get the sellOrderProcess which has the lowest orderNum)
        //and update its status as processing
        var NextProcessList = await _dbSellOrderProcess.FromSqlRaw(
            "UPDATE \"SellOrderJobs\" SET \"Status\" = 4 WHERE \"OrderNum\" = (SELECT MIN(\"OrderNum\") FROM \"SellOrderJobs\" WHERE \"Status\" IN (1, 2) LIMIT 1)RETURNING *\n").ToListAsync();
        //Console.WriteLine("id of the selected process: "+ NextProcessList[0].SellOrderProcessId);
        if (!NextProcessList.Any())
        {
            return null;
        }
        return NextProcessList[0];
    }

    public async Task<SellOrderProcess> FindAndUpdateStatus(int sellOrderProcessId, OrderStatus newStatus)
    {
        var sellOrderProcess = await _dbSellOrderProcess.FindAsync(sellOrderProcessId);
        if (sellOrderProcess == null)
        {
            return null;
        }

        if (sellOrderProcess.Status != newStatus)
        {
            sellOrderProcess.Status = newStatus;
        }

        return sellOrderProcess;
    }

    public async Task<SellOrderProcess> UpdateOrderNum(int sellOrderProcessId)
    {
        var sellOrderProcess = await _dbSellOrderProcess.FindAsync(sellOrderProcessId);
        if (sellOrderProcess == null)
        {
            return null;
        }

        sellOrderProcess.OrderNum += 10;
        return sellOrderProcess;
    }

    public async Task DeleteSellOrderProcessBySellOrderId(int sellOrderId)
    {
        var sellOrderProcess = await _dbSellOrderProcess.Where(s => s.SellOrderId == sellOrderId).FirstOrDefaultAsync();
        _dbSellOrderProcess.Remove(sellOrderProcess);
    }

    public async Task DeleteAllSellOrderProcesses()
    {
        var sellOrderProcesses = await _dbSellOrderProcess.ToListAsync();
        _dbSellOrderProcess.RemoveRange(sellOrderProcesses);
    }
}