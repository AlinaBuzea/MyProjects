using EM.Models.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Data
{
    public class ProductDB : IDatabase<Product>
    {
        public enum Period
        {
            TOTALE,
            PE_LUNA,
            PE_AN
        }
        public ProductDB()
        {
            try
            {
                App.Database.databaseConn.CreateTableAsync<Product>().Wait();
            }
            catch (Exception e)
            {
                Debug.WriteLine("CreateTableAsync for Product exception:" + e.InnerException + " mesage: " + e.Message);
            }
        }

        public async Task<Product> GetAsync(int id)
        {
            return await App.Database.databaseConn.Table<Product>().Where(product => product.ProductId == id).FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetListAsync()
        {
            return await App.Database.databaseConn.Table<Product>().ToListAsync();
        }

        public async Task<List<Product>> GetListOrderedByDateAsync()
        {
            return await App.Database.databaseConn.Table<Product>()
                                                   .OrderByDescending(product => product.AquisitionDate)
                                                   .OrderBy(product => product.ProductName)
                                                   .ToListAsync();
        }

        public async Task<int> SaveAsync(Product product)
        {
            if (product.ProductId != 0)
            {
                return await App.Database.databaseConn.UpdateAsync(product);
            }

            return await App.Database.databaseConn.InsertAsync(product);
        }

        public async Task<int> DeleteAsync(Product product)
        {
            return await App.Database.databaseConn.DeleteAsync(product);
        }

        public static Period StringToPeriod(string periodString)
        {
            return periodString.Equals(nameof(Period.TOTALE)) ? Period.TOTALE :  
                (periodString.Equals(nameof(Period.PE_LUNA)) ? Period.PE_LUNA : Period.PE_AN);
        }

        public static string PeriodToString(Period period)
        {
            return period.Equals(Period.TOTALE) ? nameof(Period.TOTALE) :
                (period.Equals(Period.PE_LUNA) ? nameof(Period.PE_LUNA) : nameof(Period.PE_AN));
        }

        public async Task<List<Product>> GetProductsWithTheGivenCategoryOrderByDateAsync(int categId)
        {
            return await App.Database.databaseConn.Table<Product>()
                                                    .Where(product => product.ProdCategoryId == categId)
                                                    .OrderByDescending(product=> product.AquisitionDate)
                                                    .ToListAsync();
        }

        public async Task<List<Product>> GetProductsWithTheGivenAquisitionShopAsync(int aquisitionShopId)
        {
            return await App.Database.databaseConn.Table<Product>().Where(product => product.AquisitionShopId == aquisitionShopId).ToListAsync();
        }

        public async Task<List<Product>> GetProductsBoughtInTimePeriodAsync(DateTime limitDay)
        {
            return await App.Database.databaseConn.Table<Product>()
               .Where(product => product.AquisitionDate >= limitDay).ToListAsync();
        }

        public async Task<List<Product>> GetProductsWithTheGivenCategoryAndBoughtInTimePeriodAsync(int categId, DateTime limitDay)
        {
            return await App.Database.databaseConn.Table<Product>()
               .Where(product => product.ProdCategoryId == categId && product.AquisitionDate >= limitDay).ToListAsync();
        }

        public async Task<List<Product>> GetProductsWithTheGivenCategoryAndBoughtInMonthYearAsync(int categId, int limitYear, int? limitMonth = null)
        {
            List<Product> requestedProducts = await App.Database.databaseConn.Table<Product>()
                                                    .Where(product => product.ProdCategoryId == categId).ToListAsync();

            List<Product> filteredlist = new List<Product>();
            foreach (Product product1 in requestedProducts)
            {
                if (limitMonth.HasValue)
                {
                    if (product1.AquisitionDate.Month.Equals(limitMonth) && product1.AquisitionDate.Year == limitYear)
                        filteredlist.Add(product1);
                }
                else
                {
                    if (product1.AquisitionDate.Year == limitYear)
                        filteredlist.Add(product1);
                }
            }
            return filteredlist.OrderByDescending(product=>product.AquisitionDate).ToList();
        }

        public async Task<double> GetTotalPriceFromProductsWithTheGivenCategoryAndBoughtInMonthYearAsync(int categId, int limitYear, int? limitMonth = null)
        {
            List<Product> filteredlist = await GetProductsWithTheGivenCategoryAndBoughtInMonthYearAsync(categId, limitYear, limitMonth);

            return filteredlist.Count == 0 ? 0 : filteredlist.Sum(product => product.Price);
        }

        public async Task<double> GetTotalPriceFromProductsBoughtDuringMonthAndYearAsync(int limitMonth, int limitYear)
        {
            List<Product> requestedProducts = await App.Database.databaseConn.Table<Product>().ToListAsync();

            List<Product> filteredlist = new List<Product>();
            foreach (Product product1 in requestedProducts)
            {
                if (product1.AquisitionDate.Month.Equals(limitMonth) && product1.AquisitionDate.Year == limitYear)
                {
                    filteredlist.Add(product1);
                    //Console.WriteLine("Id:" + product1.ProductId + " Name:" + product1.ProductName + " Price:" +
                    //        product1.Price + " Date:" + product1.AquisitionDate.ToString());
                    
                }
            }
            return filteredlist.Count == 0 ? 0 : filteredlist.Sum(product => product.Price);
        }

        public async Task<double> GetTotalPriceFromProductsBoughtDuringYearAsync(int limitYear)
        {
            List<Product> requestedProducts = await App.Database.databaseConn.Table<Product>().ToListAsync();

            List<Product> filteredlist = new List<Product>();
            foreach (Product product1 in requestedProducts)
            {
                if (product1.AquisitionDate.Year == limitYear)
                {
                    filteredlist.Add(product1);
                    //Console.WriteLine("Id:" + product1.ProductId + " Name:" + product1.ProductName + " Price:" +
                    //        product1.Price + " Date:" + product1.AquisitionDate.ToString());

                }
            }
            return filteredlist.Count == 0 ? 0 : filteredlist.Sum(product => product.Price);
        }

        public async Task<double> GetTotalPriceFromProductsAsync()
        {
            List<Product> requestedProducts = await App.Database.databaseConn.Table<Product>().ToListAsync();
            //foreach(Product product1 in requestedProducts)
            //{
            //    Console.WriteLine("Id:" + product1.ProductId + " Name:" + product1.ProductName + " Price:" + 
            //        product1.Price + " Date:" + product1.AquisitionDate.ToString());
            //}
            return requestedProducts.Count == 0 ? 0 : requestedProducts.Sum(product => product.Price) ;
        }
    }
}
