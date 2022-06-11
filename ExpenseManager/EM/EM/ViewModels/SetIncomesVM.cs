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

namespace EM.ViewModels
{
    public class SetIncomesVM : BaseViewModel
    {
        #region Fields
        private int id;
        private double incomeValue;
        private string month;
        private int year;

        private MonthYearVM currentMonthYearVM;
        private bool isCurrentMonth;
        private readonly IncomeDB incomeDB;
        #endregion

        #region Commands
        public ICommand InformationCommand { get; }
        public ICommand AddIncomeToTheCurrentValueCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand SaveBudgetPlanCommand { get; }
        #endregion

        public SetIncomesVM()
        {
            InformationCommand = new Command(OnInformationCommand);
            AddIncomeToTheCurrentValueCommand = new Command(OnAddIncomeToTheCurrentValueCommand);
            CancelCommand = new Command(OnCancelCommand);
            SaveBudgetPlanCommand = new Command(OnSaveProductsCommand);

            currentMonthYearVM = new MonthYearVM();
            incomeDB = new IncomeDB();
            Month = currentMonthYearVM.Months[DateTime.Today.Month - 1];
            Year = DateTime.Today.Year;
        }
        #region Properties
        public int Id
        {
            get => id;
        }

        public double IncomeValue
        {
            get => incomeValue;
            set => SetProperty(ref incomeValue, value);
        }

        public string Month
        {
            get => month;
            set
            {
                SetProperty(ref month, value);
                Income income = Task.Run(async () => await incomeDB.GetByMonthYearAsync(Month, Year)).Result;
                IncomeValue = income == null ? 0 : income.Value;
                IsCurrentMonth = Month.Equals(currentMonthYearVM.Months[DateTime.Today.Month - 1]) && Year == DateTime.Today.Year;
            }
        }

        public int Year
        {
            get => year;
            set
            {
                SetProperty(ref year, value);
                Income income = Task.Run(async () => await incomeDB.GetByMonthYearAsync(Month, Year)).Result;
                IncomeValue = income == null ? 0 : income.Value;
                IsCurrentMonth = Month.Equals(currentMonthYearVM.Months[DateTime.Today.Month - 1]) && year == DateTime.Today.Year;
            }
        }
        public bool IsCurrentMonth
        {
            get => isCurrentMonth;
            set => SetProperty(ref isCurrentMonth, value);
        }

        public MonthYearVM CurrentMonthYearVM
        {
            get => currentMonthYearVM;
            set => SetProperty(ref currentMonthYearVM, value);
        }
        #endregion

        private void OnInformationCommand()
        {
            Device.BeginInvokeOnMainThread(() =>
                Application.Current.MainPage.DisplayAlert("Instructiuni", "Pentru a adauga o suma de bani, apasati pe \"+\"", "OK"));
        }

        private async void OnAddIncomeToTheCurrentValueCommand()
        {
        Introduce_Income:
            string sumOfMoneyString = "0";
            try
            {
                sumOfMoneyString = await Application.Current.MainPage.DisplayPromptAsync("Adaugati inca o suma de bani la venitul actual", "Introduceti suma:", cancel: "Anuleaza", keyboard: Keyboard.Numeric);///merge pe navigation page(nu stiu daca si pe shell)
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: message:" + e.Message + " inner:" + e.InnerException);
            }

            sumOfMoneyString = sumOfMoneyString ?? "0"; //https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-coalescing-operator
            try
            {
                double sumOfMoney = Double.Parse(sumOfMoneyString);
                IncomeValue += sumOfMoney;

            }
            catch (FormatException)
            {
                await Application.Current.MainPage.DisplayAlert("Eroare", "Suma de bani trebuie sa fie numerica. Introduceti un numar!", "OK");
                goto Introduce_Income;
            }

        }

        private async void OnCancelCommand()
        {
            await App.Current.MainPage.Navigation.PopAsync();//BudgetMenuPage
        }
        private async void OnSaveProductsCommand()
        {
            Income income = await incomeDB.GetByValueAndMonthYearAsync(IncomeValue, Month, Year);
            if (income == null)
            {
                income = new Income();
                income.Value = IncomeValue;
                income.Month = Month;
                income.Year = Year;
            }
            else
            {
                income.Value = IncomeValue;
            }
            await incomeDB.SaveAsync(income);

            await App.Current.MainPage.Navigation.PopAsync();//BudgetMenuPage
        }
    }
}
