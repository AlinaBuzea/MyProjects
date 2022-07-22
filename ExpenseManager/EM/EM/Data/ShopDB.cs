using EM.Models.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace EM.Data
{
    public class ShopDB : IDatabase<Shop>
    {
        public ShopDB()
        {
            try
            {
                App.Database.databaseConn.CreateTableAsync<Shop>().Wait();
            }
            catch (Exception e)
            {
                Debug.WriteLine("CreateTableAsync for Shop exception:" + e.InnerException + " mesage: " + e.Message);
            }
        }

        public async Task<Shop> GetAsync(int id)
        {
            return await App.Database.databaseConn.Table<Shop>().Where(shop => shop.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> GetIdByNameAndAddressAsync(string shopName, string address)
        {
            Shop shop = await App.Database.databaseConn.Table<Shop>()
                                                        .Where(shop1 => shop1.ShopName.ToUpper().Equals(shopName.ToUpper())
                                                        && shop1.ShopAddress.ToUpper().Equals(address.ToUpper()))
                                                        .FirstOrDefaultAsync();
            return shop == null ? 0 : Int32.Parse(shop.Id.ToString());
        }

        public async Task<List<Shop>> GetListAsync()
        {
            return await App.Database.databaseConn.Table<Shop>().ToListAsync();
        }

        public async Task<int> SaveAsync(Shop shop)
        {
            if (shop.Id == 0)
            {
                return await App.Database.databaseConn.InsertAsync(shop);
            }
            return await App.Database.databaseConn.UpdateAsync(shop);
        }

        public async Task<int> DeleteAsync(Shop shop)
        {
            return await App.Database.databaseConn.DeleteAsync(shop);
        }
    }
}
