using EM.Models.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace EM.Data
{
    public class CategoryDB : IDatabase<Category>
    {
        public CategoryDB()
        {
            try
            {
                App.Database.databaseConn.CreateTableAsync<Category>().Wait();
                List<Category> categories = Task.Run(async () => await GetListAsync()).Result;
                if (categories.Count == 0) { InitializeNewTable(); }
            }
            catch (Exception e)
            {
                Debug.WriteLine("CreateTableAsync for Category exception:" + e.InnerException + " mesage: " + e.Message);
            }
        }

        private void InitializeNewTable()
        {
            Task.Run(async () =>
            {
                List<Category> implicitCategories = new List<Category>()
                {
                    new Category(){CategoryName="Alimente", OrderIndex = -1},
                    new Category(){CategoryName="Facturi", OrderIndex = -1},
                    new Category(){CategoryName="Imbracaminte", OrderIndex = -1},
                    new Category(){CategoryName="Incaltaminte", OrderIndex = -1},
                    new Category(){CategoryName="Hobby", OrderIndex = -1},
                    new Category(){CategoryName="Sanatate", OrderIndex = -1},
                    new Category(){CategoryName="Igiena", OrderIndex = -1},
                    new Category(){CategoryName="Gadget-uri", OrderIndex = -1}
                };

                foreach (Category category in implicitCategories)
                {
                    await SaveAsync(category);
                }
            }).Wait();
        }

        public async Task<Category> GetAsync(int id)
        {
            return await App.Database.databaseConn.Table<Category>().Where(category => category.CategoryId == id).FirstOrDefaultAsync();
        }

        public async Task<Category> GetByNameAsync(string categoryName)
        {
            return await App.Database.databaseConn.Table<Category>()
                                                    .Where(category => category.CategoryName.ToUpper().Equals(categoryName.ToUpper()))
                                                    .FirstOrDefaultAsync();
        }

        public async Task<int> GetIdByNameAsync(string categoryName)
        {
            Category categ = await App.Database.databaseConn.Table<Category>()
                                                                .Where(category => category.CategoryName.ToUpper().Equals(categoryName.ToUpper()))
                                                                .FirstOrDefaultAsync();
            return categ == null ? 0 : Int32.Parse(categ.CategoryId.ToString());
        }

        public async Task<List<Category>> GetListAsync()
        {
            return await App.Database.databaseConn.Table<Category>().ToListAsync();
        }

        public async Task<int> SaveAsync(Category category)
        {
            if (category.CategoryId == 0)
            {
                return await App.Database.databaseConn.InsertAsync(category);
            }
            return await App.Database.databaseConn.UpdateAsync(category);
        }

        public async Task<int> DeleteAsync(Category category)
        {
            return await App.Database.databaseConn.DeleteAsync(category);
        }
    }
}
