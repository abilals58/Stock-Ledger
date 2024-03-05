using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace LedgerTest;

public class DatabaseFixture : IDisposable
{
    public ApiDbContext _dbContext { get; private set; }
    public DatabaseFixture()
    {
        // Set up in-memory database
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        var options = new DbContextOptionsBuilder<ApiDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .UseInternalServiceProvider(serviceProvider)
            .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        
        _dbContext = new ApiDbContext(options);
        
        //seed data
        SeedData();
        
    }
    
    private void SeedData()
    {
        // Seed your in-memory database with test data
        //add users
        var user1 = new User(1, "Ahmet", "Yildiz", "ayildiz", "ayildiz@gmail.com", "65984943", "5343519032", 5000);
        var user2 = new User(2, "Yusuf", "Colak", "ycolak", "ycolak@gmail.com", "7593203", "546328792", 10000);
        var user3 = new User(3, "Yavuz", "Colak", "yazcolak", "yazcolak@gmail.com", "658932032", "5468232022", 1000);

        _dbContext.Users.Add(user1);
        _dbContext.Users.Add(user2);
        _dbContext.Users.Add(user3);
        
        //add stocks
        var stock1 = new Stock(1, "THY", 1000, 10, 0, 30, 30, 30, true);
        var stock2 = new Stock(2, "TCELL", 10000, 15, 0, 25, 25, 25, true);

        _dbContext.Stocks.Add(stock1);
        _dbContext.Stocks.Add(stock2);

        _dbContext.SaveChanges();

    }

    public void Dispose()
    {
        _dbContext.Dispose();
        // ... clean up test data from the database ...
    }
}
