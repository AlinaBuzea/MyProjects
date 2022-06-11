using EM.Data;
using EM.Models.Entity;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EM.ViewModels.SubViewModels
{
    public class BudgetVM : BaseViewModel
    {
        #region Fields
        private double alocatedBudget;
        private double currentValue;
        private bool isLimitNotificationActive;
        private double limitNotificationValue;
        private string month;
        private int year;
        private Category budgetCategory;
        private MonthYearVM currentMonthYearVM;

        private bool isValidAlocatedBudgetValue;
        private bool isNotValidAlocatedBudgetValue;
        private bool isValidNotificationValue;
        private bool isNotValidNotificationValue;
        #endregion

        public BudgetVM()
        {
            currentMonthYearVM = new MonthYearVM();
            AlocatedBudget = 0;
            CurrentValue = 0;
            IsLimitNotificationActive = false;
            LimitNotificationValue = 0;
            Month = currentMonthYearVM.Months[DateTime.Today.Month - 1];
            Year = DateTime.Today.Year;
            BudgetCategory = null;

            IsValidAlocatedBudgetValue = true;
            IsValidNotificationValue = true;
        }

        public BudgetVM(Budget budget)
        {
            currentMonthYearVM = new MonthYearVM();
            AlocatedBudget = budget.AlocatedBudget;
            CurrentValue = budget.CurrentValue;
            IsLimitNotificationActive = budget.LimitNotificationValue != 0;
            LimitNotificationValue = budget.LimitNotificationValue;
            Month = budget.Month;
            Year = budget.Year;
            BudgetCategory = Task.Run(async () =>
            {
                CategoryDB categoryDB = new CategoryDB();
                return await categoryDB.GetAsync(budget.CategoryId);

            }).Result;
            IsValidAlocatedBudgetValue = true;
            IsValidNotificationValue = true;
        }
        #region Properties
        public double AlocatedBudget
        {
            get => alocatedBudget;
            set
            {
                SetProperty(ref alocatedBudget, value);
                if (alocatedBudget < 0)
                {
                    IsValidAlocatedBudgetValue = false;
                }
            }
        }
        public double CurrentValue
        {
            get => currentValue;
            set => SetProperty(ref currentValue, value);
        }

        public bool IsLimitNotificationActive
        {
            get => isLimitNotificationActive;
            set
            {
                SetProperty(ref isLimitNotificationActive, value);
                if (!isLimitNotificationActive)
                {
                    IsNotValidNotificationValue = false;
                    LimitNotificationValue = 0;
                }
            }
        }

        public double LimitNotificationValue
        {
            get => limitNotificationValue;
            set => SetProperty(ref limitNotificationValue, value);
        }

        public bool IsValidNotificationValue
        {
            get => isValidNotificationValue;
            set
            {
                SetProperty(ref isValidNotificationValue, value);
                IsNotValidNotificationValue = IsLimitNotificationActive && !IsValidNotificationValue;
            }
        }

        public bool IsNotValidNotificationValue
        {
            get => isNotValidNotificationValue;
            set
            {
                SetProperty(ref isNotValidNotificationValue, value);
            }
        }
        public bool IsValidAlocatedBudgetValue
        {
            get => isValidAlocatedBudgetValue;
            set
            {
                SetProperty(ref isValidAlocatedBudgetValue, value);
                IsNotValidAlocatedBudgetValue = !isValidAlocatedBudgetValue;
            }
        }

        public bool IsNotValidAlocatedBudgetValue
        {
            get => isNotValidAlocatedBudgetValue;
            set => SetProperty(ref isNotValidAlocatedBudgetValue, value);
        }
        public string Month
        {
            get => month;
            set => SetProperty(ref month, value);
        }

        public int Year
        {
            get => year;
            set => SetProperty(ref year, value);
        }

        public Category BudgetCategory
        {
            get => budgetCategory;
            set => SetProperty(ref budgetCategory, value);
        }

        public MonthYearVM CurrentMonthYearVM
        {
            get => currentMonthYearVM;
            set => SetProperty(ref currentMonthYearVM, value);
        }
        #endregion
    }
}
