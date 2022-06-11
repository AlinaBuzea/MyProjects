using EM.Models.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace EM.Data
{
    public class BudgetDB : IDatabase<Budget>
    {
        public BudgetDB()
        {
            try
            {
                App.Database.databaseConn.CreateTableAsync<Budget>().Wait();
            }
            catch (Exception e)
            {
                Debug.WriteLine("CreateTableAsync for Budget exception:" + e.InnerException + " mesage: " + e.Message);
            }
        }

        public async Task<Budget> GetAsync(int id)
        {
            return await App.Database.databaseConn.Table<Budget>().Where(budget => budget.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Budget> GetByCategoryMonthYearAsync(int cathegoryId, string month, int year)
        {
            return await App.Database.databaseConn.Table<Budget>()
                                                  .Where(budget => budget.Month.Equals(month) &&
                                                                     budget.Year == year &&
                                                                     budget.CategoryId == cathegoryId)
                                                  .FirstOrDefaultAsync();
        }

        public async Task<List<Budget>> GetListAsync()
        {
            return await App.Database.databaseConn.Table<Budget>().ToListAsync();
        }

        public async Task<List<Budget>> GetListByMonthYearAsync(string month, int year)
        {
            List<Budget> list = await App.Database.databaseConn.Table<Budget>().ToListAsync();
            List<Budget> filteredlist = new List<Budget>();
            foreach (Budget budget in list)
            {
                if (budget.Month.Equals(month) && budget.Year == year)
                    filteredlist.Add(budget);
            }
            return filteredlist;
        }

        public async Task<int> SaveAsync(Budget budget)
        {
            Budget budgetOnMonthYear = await GetByCategoryMonthYearAsync(budget.CategoryId,
                                        budget.Month, budget.Year);
            if (budget.Id == 0)
            {
                if (budgetOnMonthYear == null)
                {
                    return await App.Database.databaseConn.InsertAsync(budget);
                }
                budget.Id = budgetOnMonthYear.Id;
            }

            if (!budget.Equals(budgetOnMonthYear))
            {
                return await App.Database.databaseConn.UpdateAsync(budget);
            }
            return 0;
        }

        public async Task<int> DeleteAsync(Budget budget)
        {
            return await App.Database.databaseConn.DeleteAsync(budget);
        }
    }
}
