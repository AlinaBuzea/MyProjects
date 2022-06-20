using EM.Data;
using EM.Models.Entity;
using EM.ViewModels.SubViewModels;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace EM.ViewModels
{
    public class BudgetPlanerVM : BaseViewModel
    {
        #region Fields
        private string month;
        private int year;
        private List<BudgetVM> budgets;
        private readonly BudgetDB budgetDB;
        private MonthYearVM currentMonthYearVM;
        #endregion

        #region Commands
        public ICommand InformationCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand SaveBudgetPlanCommand { get; }
        #endregion

        public BudgetPlanerVM()
        {
            budgetDB = new BudgetDB();
            currentMonthYearVM = new MonthYearVM();

            InformationCommand = new Command(OnInformationCommand);
            CancelCommand = new Command(OnCancelCommand);
            SaveBudgetPlanCommand = new Command(OnSaveBudgetPlanCommand);

            month = currentMonthYearVM.Months[DateTime.Today.Month - 1];
            Year = DateTime.Today.Year;
        }

        #region Properties
        public string Month
        {
            get => month;
            set
            {
                SetProperty(ref month, value);
                InitializeBudgetsList();
            }
        }

        public int Year
        {
            get => year;
            set
            {
                SetProperty(ref year, value);
                InitializeBudgetsList();
            }
        }

        public List<BudgetVM> Budgets
        {
            get => budgets;
            set => SetProperty(ref budgets, value);
        }

        public MonthYearVM CurrentMonthYearVM
        {
            get => currentMonthYearVM;
            set => SetProperty(ref currentMonthYearVM, value);
        }
        #endregion

        private void InitializeBudgetsList()
        {
            Budgets = Task.Run(async () =>
            {
                CategoryDB categoryDB = new CategoryDB();
                List<Category> categories = await categoryDB.GetListAsync();
                List<BudgetVM> budgetVMs = new List<BudgetVM>();
                foreach (Category category in categories)
                {
                    Budget existentBudget = await budgetDB.GetByCategoryMonthYearAsync(category.CategoryId, Month, Year);
                    if (existentBudget == null)
                    {
                        budgetVMs.Add(new BudgetVM() { BudgetCategory = new Category() { CategoryId = category.CategoryId, CategoryName = category.CategoryName },
                                                      AlocatedBudget = 0, LimitNotificationValue = 0 });
                    }
                    else
                    {
                        budgetVMs.Add(new BudgetVM(existentBudget));
                    }
                }

                return budgetVMs;
            }).Result;

            Task.Run(() =>
            {
                foreach (BudgetVM budget in Budgets)
                {
                    Console.WriteLine("BudgetCategory: " + budget.BudgetCategory.CategoryName + "; AlocatedBudget: " + budget.AlocatedBudget +
                        "; IsLimitNotificationActive: " + budget.LimitNotificationValue + "; AlocatedBudget: " + budget.LimitNotificationValue
                        + "; Month: " + budget.Month + "; Year: " + budget.Year);
                }

            });
        }

        private void OnInformationCommand()
        {
            Device.BeginInvokeOnMainThread(() =>
                App.Current.MainPage.DisplayAlert("Instructiuni", "Limita minima: cand se atinge aceasta limita, veti primi o notificare" +
                "care va va informa ca bugetul alocat per categoria respectiva este aproape de epuizare", "OK"));
        }

        public async void OnSaveBudgetPlanCommand()
        {
            foreach (BudgetVM budgetVM in budgets)
            {
                if (budgetVM.IsNotValidNotificationValue || budgetVM.IsNotValidAlocatedBudgetValue)
                {
                    await App.Current.MainPage.DisplayAlert("Eroare", $"Date invalide la categoria {budgetVM.BudgetCategory.CategoryName}!", "OK");
                    return;
                }
            }
            List<Budget> budgets1 = await budgetDB.GetListAsync();
            _ = Task.Run(() =>
            {
                foreach (Budget newBudget in budgets1)
                {
                    Console.WriteLine("BudgetCategory: " + newBudget.CategoryId + "; AlocatedBudget: " + newBudget.AlocatedBudget.ToString() +
                           "; IsLimitNotificationActive: " + newBudget.LimitNotificationValue.ToString() + "; CurrentValue: " + newBudget.CurrentValue.ToString()
                           + "; MonthYear: " + newBudget.Month + "; MonthYear: " + newBudget.Year);
                }
            });
            ProductDB productDB = new ProductDB();

            foreach (BudgetVM budgetVM in budgets)
            {
                Budget newBudget = new Budget();
                newBudget.CategoryId = budgetVM.BudgetCategory.CategoryId;
                newBudget.CurrentValue = await productDB.GetTotalPriceFromProductsWithTheGivenCategoryAndBoughtInMonthYearAsync(newBudget.CategoryId, Year, currentMonthYearVM.Months.IndexOf(Month));
                newBudget.AlocatedBudget = budgetVM.AlocatedBudget;
                newBudget.LimitNotificationValue = budgetVM.IsLimitNotificationActive ? budgetVM.LimitNotificationValue : 0;
                newBudget.Month = Month;
                newBudget.Year = Year;

                await budgetDB.SaveAsync(newBudget);
               
                Console.WriteLine("BudgetCategory: " + newBudget.CategoryId + "; AlocatedBudget: " + newBudget.AlocatedBudget.ToString() +
                        "; IsLimitNotificationActive: " + newBudget.LimitNotificationValue.ToString() + "; CurrentValue: " + newBudget.CurrentValue.ToString()
                        + "; MonthYear: " + newBudget.Month + "; MonthYear: " + newBudget.Year);
            }
            await App.Current.MainPage.DisplayAlert("Informare", "Setarile au fost salvate!", "OK");
            InitializeBudgetsList();
        }

        private async void OnCancelCommand()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
