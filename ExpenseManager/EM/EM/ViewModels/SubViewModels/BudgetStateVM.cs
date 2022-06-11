using EM.Data;
using EM.Models.Entity;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EM.ViewModels.SubViewModels
{
    public class BudgetStateVM : BaseViewModel
    {
        public enum State
        {
            Good,
            CloseToLimit,
            OverLimit,
            LackOfInformation // fara buget stabilit si cu limita 0
        }

        #region Fields
        private double alocatedBudget;
        private double currentValue;
        private double limitNotificationValue;
        private string month;
        private int year;
        private Category budgetCategory;

        private State currentState;
        private string currentStateString;
        private string stateImageUrl;
        private double remainingAmount;
        private MonthYearVM currentMonthYearVM;
        private readonly ProductDB productDB;
        private readonly BudgetDB budgetDB;
        #endregion

        public BudgetStateVM()
        {
            currentMonthYearVM = new MonthYearVM();
            productDB = new ProductDB();
            budgetDB = new BudgetDB();

            AlocatedBudget = 0;
            CurrentValue = 0;
            LimitNotificationValue = 0;
            BudgetCategory = new Category();
            month = currentMonthYearVM.Months[DateTime.Today.Month - 1];
            year = DateTime.Today.Year;

        }

        public BudgetStateVM(Budget budget)
        {
            productDB = new ProductDB();
            currentMonthYearVM = new MonthYearVM();
            budgetDB = new BudgetDB();

            AlocatedBudget = budget.AlocatedBudget;
            CurrentValue = budget.CurrentValue;
            LimitNotificationValue = budget.LimitNotificationValue;
            BudgetCategory = Task.Run(async () =>
            {
                CategoryDB categoryDB = new CategoryDB();
                return await categoryDB.GetAsync(budget.CategoryId);
            }).Result;
            month = budget.Month;
            year = budget.Year;
        }

        #region Properties
        public double AlocatedBudget
        {
            get => alocatedBudget;
            set
            {
                SetProperty(ref alocatedBudget, value);
                RemainingAmount = alocatedBudget - currentValue;
            }
        }
        public double CurrentValue
        {
            get => currentValue;
            set
            {
                SetProperty(ref currentValue, value);
                RemainingAmount = alocatedBudget - currentValue;
            }
        }

        public double LimitNotificationValue
        {
            get => limitNotificationValue;
            set => SetProperty(ref limitNotificationValue, value);
        }

        public string Month
        {
            get => month;
            set
            {
                SetProperty(ref month, value);
                SetFieldsByMonthAndYear();
            }
        }

        public int Year
        {
            get => year;
            set
            {
                SetProperty(ref year, value);
                SetFieldsByMonthAndYear();
            }
        }

        public Category BudgetCategory
        {
            get => budgetCategory;
            set => SetProperty(ref budgetCategory, value);
        }

        public double RemainingAmount
        {
            get => remainingAmount;
            set
            {
                SetProperty(ref remainingAmount, value);
                CurrentState = remainingAmount > limitNotificationValue ? State.Good :
                                (remainingAmount == limitNotificationValue ? State.CloseToLimit : State.OverLimit);

            }
        }

        public State CurrentState
        {
            get => currentState;
            set
            {
                SetProperty(ref currentState, value);
                ReinitializeStateValues();
            }
        }

        public string CurrentStateString
        {
            get => currentStateString;
            set => SetProperty(ref currentStateString, value);
        }
        public string StateImageUrl
        {
            get => stateImageUrl;
            set => SetProperty(ref stateImageUrl, value);
        }

        public MonthYearVM CurrentMonthYearVM
        {
            get => currentMonthYearVM;
            set => SetProperty(ref currentMonthYearVM, value);
        }
        #endregion

        private void SetFieldsByMonthAndYear()
        {
            var budget = Task.Run(async () =>
                                await budgetDB.GetByCategoryMonthYearAsync(BudgetCategory.CategoryId, month, year)
                                ).Result;
            LimitNotificationValue = budget == null ? 0 : budget.LimitNotificationValue;
            alocatedBudget = 0;
            CurrentValue = Task.Run(async () => await productDB.GetTotalPriceFromProductsWithTheGivenCategoryAndBoughtInMonthYearAsync(
                   budgetCategory.CategoryId, Year, currentMonthYearVM.Months.IndexOf(Month) + 1)
                   ).Result;

            AlocatedBudget = budget == null ? 0 : budget.AlocatedBudget;
        }
        private void ReinitializeStateValues()
        {
            switch (currentState)
            {
                case State.Good:
                    StateImageUrl = "GoodState.png";
                    CurrentStateString = "Buna";
                    break;
                case State.CloseToLimit:
                    StateImageUrl = "CloseToLimitState.png";
                    CurrentStateString = "La limita";
                    break;
                case State.OverLimit:
                    StateImageUrl = "OverLimitState.png";
                    CurrentStateString = "Depasit";
                    break;

            }
        }
    }
}
