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
    public class SavingsVM : BaseViewModel
    {
        #region Fields
        private double savingsValue;
        private string month;
        private int year;
        private MonthYearVM currentMonthYearVM;

        private string selection;
        private string descriptionOfSavings;
        private string descriptionOfMonthYear;

        private bool isVisibleMonth;
        private bool isVisibleYear;
        private bool isVisibleMonthYearText;

        private readonly ProductDB productDB;
        private readonly IncomeDB incomeBD;

        #endregion

        public ICommand InformationCommand { get; }

        public SavingsVM()
        {
            productDB = new ProductDB();
            incomeBD = new IncomeDB();
            InformationCommand = new Command(OnInformationCommand);
            currentMonthYearVM = new MonthYearVM();
            selection = "Total";
            Month = currentMonthYearVM.Months[DateTime.Today.Month - 1];
            Year = DateTime.Today.Year;
        }

        #region Properties
        public double SavingsValue
        {
            get => savingsValue;
            set => SetProperty(ref savingsValue, value);
        }

        public string Month
        {
            get => month;
            set
            {
                SetProperty(ref month, value);
                SelectionChanged();
            }
        }

        public int Year
        {
            get => year;
            set
            {
                SetProperty(ref year, value);
                SelectionChanged();
            }
        }
        public bool IsVisibleMonth
        {
            get => isVisibleMonth;
            set => SetProperty(ref isVisibleMonth, value);
        }
        public bool IsVisibleYear
        {
            get => isVisibleYear;
            set => SetProperty(ref isVisibleYear, value);
        }

        public bool IsVisibleMonthYearText
        {
            get => isVisibleMonthYearText;
            set => SetProperty(ref isVisibleMonthYearText, value);
        }

        public string DescriptionOfMonthYear
        {
            get => descriptionOfMonthYear;
            set => SetProperty(ref descriptionOfMonthYear, value);
        }
        public string DescriptionOfSavings
        {
            get => descriptionOfSavings;
            set => SetProperty(ref descriptionOfSavings, value);
        }

        public string Selection
        {
            get => selection;
            set
            {
                SetProperty(ref selection, value);
                SelectionChanged();
            }
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

        private void SelectionChanged()
        {
            if (selection.Equals("Total"))
            {
                DescriptionOfSavings = "Totalul de economii: ";
                IsVisibleMonth = false;
                IsVisibleYear = false;
                SavingsValue = Task.Run(async () =>
                {
                    double expensesCost = await productDB.GetTotalPriceFromProductsAsync();
                    double incomes = await incomeBD.GetTotalValueAsync();
                    return incomes - expensesCost;
                }).Result;
            }
            else if (selection.Equals("MonthYear"))
            {
                DescriptionOfSavings = "Suma de bani economisita in luna " + Month + " anul " + Year + ": ";
                DescriptionOfMonthYear = "Selectati luna si anul:";
                IsVisibleMonth = true;
                IsVisibleYear = true;
                SavingsValue = Task.Run(async () =>
                {
                    double expensesCost = await productDB.GetTotalPriceFromProductsBoughtDuringMonthAndYearAsync(currentMonthYearVM.Months.IndexOf(Month) + 1,
                                                                                                                Year);
                    double incomes = await incomeBD.GetTotalValueByMonthAndYearAsync(Month, Year);
                    return incomes - expensesCost;
                }).Result;
            }
            else
            {
                DescriptionOfSavings = "Suma de bani economisita in anul " + Year + ": ";
                DescriptionOfMonthYear = "Selectati anul:";
                IsVisibleMonth = false;
                IsVisibleYear = true;
                SavingsValue = Task.Run(async () =>
                {
                    double expensesCost = await productDB.GetTotalPriceFromProductsBoughtDuringYearAsync(Year);
                    double incomes = await incomeBD.GetTotalValueByYearAsync(Year);
                    return incomes - expensesCost;
                }).Result;
            }
            IsVisibleMonthYearText = IsVisibleMonth || IsVisibleYear;
            SavingsValue= Math.Round(savingsValue, 2);
        }
    }
}
