using EM.Data;
using EM.Models.Entity;
using EM.ViewModels.SubViewModels;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EM.ViewModels
{
    public class BudgetActualStateVM : BaseViewModel
    {
        #region Fields
        private List<BudgetStateVM> budgetStates;
        private List<Budget> budgets;
        private readonly BudgetDB budgetDB;
        private int activeTabIndex;
        private string month;
        private int year;
        private MonthYearVM currentMonthYearVM;
        #endregion

        public BudgetActualStateVM()
        {
            budgetDB = new BudgetDB();
            currentMonthYearVM = new MonthYearVM();
            Budgets = Task.Run(async () => await budgetDB.GetListAsync()).Result;
            ActiveTabIndex = 0;
            Month = currentMonthYearVM.Months[DateTime.Today.Month - 1];
            Year = DateTime.Today.Year;
        }

        #region Properties
        public List<BudgetStateVM> BudgetStates
        {
            get => budgetStates;
            set => SetProperty(ref budgetStates, value);
        }

        public List<Budget> Budgets
        {
            get => budgets;
            set
            {
                SetProperty(ref budgets, value);
                InitializeBudgetStates();
            }
        }
        public int ActiveTabIndex
        {
            get => activeTabIndex;
            set
            {
                SetProperty(ref activeTabIndex, value);
                Console.WriteLine("Active tab: " + activeTabIndex);
            }
        }

        public string Month
        {
            get => month;
            set
            {
                SetProperty(ref month, value);
                Budgets = Task.Run(async () => await budgetDB.GetListByMonthYearAsync(month, year)).Result;
            }
        }

        public int Year
        {
            get => year;
            set
            {
                SetProperty(ref year, value);
                Budgets = Task.Run(async () => await budgetDB.GetListByMonthYearAsync(month, year)).Result;
            }
        }

        public MonthYearVM CurrentMonthYearVM
        {
            get => currentMonthYearVM;
            set => SetProperty(ref currentMonthYearVM, value);
        }
        #endregion

        private void InitializeBudgetStates()
        {
            List<BudgetStateVM> list = new List<BudgetStateVM>();
            if (budgets.Count == 0)
            {
                var categories = Task.Run(async () =>
                {
                    CategoryDB categoryDB = new CategoryDB();
                    return await categoryDB.GetListAsync();
                }).Result;
                foreach (Category category in categories)
                {
                    BudgetStateVM budgetState = new BudgetStateVM
                    {
                        BudgetCategory = new Category
                        {
                            CategoryId = category.CategoryId,
                            CategoryName = category.CategoryName,
                            OrderIndex = category.OrderIndex
                        },
                        Month = month,
                        Year = year

                    };
                    list.Add(budgetState);
                }
            }
            foreach (Budget budget in budgets)
            {
                BudgetStateVM budgetState = new BudgetStateVM(budget);
                list.Add(budgetState);
            }

            BudgetStates = new List<BudgetStateVM>(list);
        }
    }
}
