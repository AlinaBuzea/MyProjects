using EM.Models.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Data
{
    public class IncomeDB : IDatabase<Income>
    {
        public IncomeDB()
        {
            try
            {
                App.Database.databaseConn.CreateTableAsync<Income>().Wait();
            }
            catch (Exception e)
            {
                Debug.WriteLine("CreateTableAsync for Income exception:" + e.InnerException + " mesage: " + e.Message);
            }
        }

        public async Task<Income> GetAsync(int id)
        {
            return await App.Database.databaseConn.Table<Income>().Where(income => income.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Income> GetByValueAndMonthYearAsync(double incomeValue, string month, int year)
        {
            return await App.Database.databaseConn.Table<Income>()
                                                   .Where(income => income.Value == incomeValue &&
                                                          income.Month.Equals(month) && income.Year == year)
                                                    .FirstOrDefaultAsync();
        }

        public async Task<Income> GetByMonthYearAsync(string month, int year)
        {
            return await App.Database.databaseConn.Table<Income>()
                                                   .Where(income => income.Month.Equals(month) && income.Year == year)
                                                    .FirstOrDefaultAsync();
        }

        public async Task<List<Income>> GetListAsync()
        {
            return await App.Database.databaseConn.Table<Income>().OrderByDescending(income => income.Month)
                                                                    .OrderByDescending(income => income.Year)
                                                                    .ToListAsync();
        }

        public async Task<int> SaveAsync(Income income)
        {
            Income income1 = await GetByMonthYearAsync(income.Month, income.Year);
            if (income1 != null && income.Id == 0)
            {
                income1.Value = income.Value;
                return await App.Database.databaseConn.UpdateAsync(income1);
            }

            if (income.Id == 0 && income1 == null)
            {
                return await App.Database.databaseConn.InsertAsync(income);
            }
            return await App.Database.databaseConn.UpdateAsync(income);
        }


        public async Task<int> DeleteAsync(Income income)
        {
            return await App.Database.databaseConn.DeleteAsync(income);
        }


        public async Task<double> GetTotalValueAsync()
        {
            List<Income> incomes = await App.Database.databaseConn.Table<Income>().ToListAsync();
            foreach (Income income1 in incomes)
            {
                Console.WriteLine("Id:" + income1.Id + " Value" + income1.Value + " Month:" + income1.Month + " Year:" + income1.Year);
            }
            return incomes.Count == 0 ? 0 : incomes.Sum(income => income.Value);
        }

        public async Task<double> GetTotalValueByMonthAndYearAsync(string limitMonth, int limitYear)
        {
            List<Income> incomes = await App.Database.databaseConn.Table<Income>()
                                                                  .Where(income => income.Month.Equals(limitMonth) && income.Year == limitYear)
                                                                  .ToListAsync();
            return incomes.Count == 0 ? 0 : incomes.Sum(income => income.Value);
        }
        public async Task<double> GetTotalValueByYearAsync(int limitYear)
        {
            List<Income> incomes = await App.Database.databaseConn.Table<Income>()
                                                                  .Where(income => income.Year == limitYear)
                                                                  .ToListAsync();
            return incomes.Count == 0 ? 0 : incomes.Sum(income => income.Value);
        }
    }
}
