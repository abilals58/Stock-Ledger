using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Ledger.Ledger.Web.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> AddUserAsync(User user);
        Task<User> UpdateUserAsync(int id, User newUser);
        Task<User> DeleteUserAsync(int id);
        Task<User> UpdateBudget(int? id, char sign, double amount);
    }
    
    public class UserRepository :IUserRepository // User service corresponds to data access tier and handles database operations
    {
        private readonly DbSet<User> _dbUser;
        private readonly IDbContext _dbContext;

        public UserRepository(IDbContext dbContext)
        {
            _dbUser = dbContext.Users;
            _dbContext = dbContext;
        }
        
        public async Task<IEnumerable<User>> GetAllUsersAsync() // returns all users 
        {
            return await _dbUser.ToListAsync();
        }
        
        public async Task<User> GetUserByIdAsync(int id) // returns a user by id, return null if there is no match
        {
            return await _dbUser.FindAsync(id);
        }
        
        public async Task<User> AddUserAsync(User user) // ads new user //TODO tüm addler için geçerli, aldıkları parametreyi returnlemesinler, işlem başarılı ya da başarısız returnlesinler
        {
            await _dbUser.AddAsync(user);
            return user;
        }
        
        public async Task<User> UpdateUserAsync(int id, User newUser) // updates a user and return that user, return null if there is no match
        {
            var user = await _dbUser.FindAsync(id);
            if (user == null) return null;
            user.Name = newUser.Name;
            user.Surname = newUser.Surname;
            user.UserName = newUser.UserName;
            user.Email = newUser.Email;
            user.Password = newUser.Password;
            user.Phone = newUser.Phone;
            return user;
        }
        
        public async Task<User> DeleteUserAsync(int id) // deletes a user, return null if there is no match
        {
            var user = await _dbUser.FindAsync(id);
            if (user == null) return null;
            _dbUser.Remove(user);
            return user;
        }

        public async Task<User> UpdateBudget(int? id, char sign, double amount)
        {
            var user = await _dbUser.FindAsync(id);
            if (sign == '+')
            {
                user.Budget += amount;
            }
            else
            {
                user.Budget -= amount;
            }

            return user;
        }
    }
}