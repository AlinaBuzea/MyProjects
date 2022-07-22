using EM.Models.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace EM.Data
{
    public class UserDB : IDatabase<User>
    {
        public UserDB()
        {
            try
            {
                App.Database.databaseConn.CreateTableAsync<User>().Wait();
            }
            catch (Exception e)
            {
                Debug.WriteLine("CreateTableAsync for User exception:" + e.InnerException + " mesage: " + e.Message);
            }
        }

        public async Task<List<User>> GetListAsync()
        {
            return await App.Database.databaseConn.Table<User>().ToListAsync();
        }

        public async Task<User> GetAsync(int id)
        {
            return await App.Database.databaseConn.Table<User>()
                            .Where(i => i.UserId == id)
                            .FirstOrDefaultAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await App.Database.databaseConn.Table<User>()
                            .Where(i => i.UserEmail.Equals(email))
                            .FirstOrDefaultAsync();
        }

        public async Task<int> SaveAsync(User user)
        {
            if (user.UserId != 0)
            {
                return await App.Database.databaseConn.UpdateAsync(user);
            }

            return await App.Database.databaseConn.InsertAsync(user);

        }

        public async Task<int> DeleteAsync(User user)
        {
            return await App.Database.databaseConn.DeleteAsync(user);
        }
    }
}