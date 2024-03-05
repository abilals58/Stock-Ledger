using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.UnitOfWork;
using Xunit.Abstractions;

namespace LedgerTest.Unit.RepositoryTests;

public class UserRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public UserRepositoryTests(DatabaseFixture fixture, ITestOutputHelper testOutputHelper)
    {
        _fixture = fixture;
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnAllUsers()
    {
        // Arrange
        var userRepository = new UserRepository(_fixture._dbContext);
            
        // Act
        var result = await userRepository.GetAllUsersAsync();
        foreach (var user in result)
        {
            _testOutputHelper.WriteLine(user.ToString());
        }
        
        // Assert
        Assert.IsAssignableFrom<IEnumerable<User>>(result); // what if it returns null, there is no users ???
        Assert.True(result.Count() == 3);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnAUser()
    {
        //Arrange
        var userRepository = new UserRepository(_fixture._dbContext);
        
        //Act
        var result = await userRepository.GetUserByIdAsync(1);
        _testOutputHelper.WriteLine(result.ToString());
        
        //Assert
        Assert.IsAssignableFrom<User>(result);
    }
    // new test
}
