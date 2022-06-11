using EM.Models.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace EM.Data
{
    public class ShoppingItemDB : IDatabase<ShoppingItem>
    {
        public ShoppingItemDB()
        {
            try
            {
                App.Database.databaseConn.CreateTableAsync<ShoppingItem>().Wait();
            }
            catch (Exception e)
            {
                Debug.WriteLine("CreateTableAsync for ShoppingItem exception:" + e.InnerException + " mesage: " + e.Message);
            }

        }

        public async Task<ShoppingItem> GetAsync(int id)
        {
            return await App.Database.databaseConn.Table<ShoppingItem>().Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ShoppingItem> GetByItemIsAPriorityAndIsBoughtAsync(string item, bool isAPriority, bool isBought)
        {
            return await App.Database.databaseConn.Table<ShoppingItem>().Where(s => s.Item.Equals(item) && s.IsAPriority == isAPriority && s.IsBought == isBought).FirstOrDefaultAsync();
        }

        public async Task<List<ShoppingItem>> GetListAsync()
        {
            return await App.Database.databaseConn.Table<ShoppingItem>().ToListAsync();
        }

        public async Task<List<ShoppingItem>> GetListOrderByIsNotBoughtThenByIsAPriorityAsync()
        {
            return await App.Database.databaseConn.Table<ShoppingItem>().OrderBy(s => s.IsBought)
                                                                        .ThenByDescending(s => s.IsAPriority)
                                                                        .ToListAsync();
        }

        public async Task<int> SaveAsync(ShoppingItem item)
        {
            if (item.Id != 0)
            {
                return await App.Database.databaseConn.UpdateAsync(item);
            }
            return await App.Database.databaseConn.InsertAsync(item);
        }

        public async Task<int> DeleteAsync(ShoppingItem item)
        {
            return await App.Database.databaseConn.DeleteAsync(item);
        }

        public void DeleteAll()
        {
            //List<ShoppingItem> list = App.Database.databaseConn.Table<ShoppingItem>().ToListAsync().Result;
            //foreach (ShoppingItem shoppingItem in list)
            //{
            //    await App.Database.databaseConn.DeleteAsync(shoppingItem);
            //}
            try
            {
                App.Database.databaseConn.DropTableAsync<ShoppingItem>().Wait();
                App.Database.databaseConn.CreateTableAsync<ShoppingItem>().Wait();
            }
            catch (Exception e)
            {
                Debug.WriteLine("CreateTableAsync for ShoppingItem exception:" + e.InnerException + " mesage: " + e.Message);
            }
        }
    }
}
