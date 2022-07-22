using EM.Data;
using EM.Models.Entity;
using EM.Pages;
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
    class ListCategoriesVM : BaseViewModel
    {
        #region Fields
        private List<CategoryListItemVM> categories;
        private CategoryDB categoryDB;
        private Category currentCategory;
        private string selectedOption;

        private string month;
        private int year;
        private MonthYearVM currentMonthYearVM;
        private bool isVisibleMonth;
        private bool isVisibleYear;

        public List<string> PeriodOption { get; }
        #endregion

        #region Commands
        public ICommand AddNewCategoryCommand { get; }
        public ICommand InformationCommand { get; }
        public ICommand TapCategoryCommand { get; }
        public ICommand DeleteSelectedCategoryCommand { get; }
        #endregion

        public ListCategoriesVM()
        {
            categoryDB = new CategoryDB();
            TapCategoryCommand = new Command<Category>(OnTapCategoryCommand);
            DeleteSelectedCategoryCommand = new Command<Category>(OnDeleteSelectedCategoryCommand);
            AddNewCategoryCommand = new Command(OnAddNewCategoryCommand);
            InformationCommand = new Command(OnInformationCommand);
            PeriodOption = new List<string>() { nameof(ProductDB.Period.TOTALE), nameof(ProductDB.Period.PE_LUNA), nameof(ProductDB.Period.PE_AN) };
            selectedOption = PeriodOption[0].ToString();
            currentMonthYearVM = new MonthYearVM();
            Month = currentMonthYearVM.Months[DateTime.Today.Month - 1];
            Year = DateTime.Today.Year;
            InitializeCategoryList();
        }

        #region Properties
        public List<CategoryListItemVM> Categories
        {
            get => categories;
            set => SetProperty(ref categories, value);
        }

        public string SelectedOption
        {
            get => selectedOption;
            set
            {
                SetProperty(ref selectedOption, value);
                if (selectedOption.Equals(nameof(ProductDB.Period.PE_LUNA)))
                {
                    IsVisibleMonth = true;
                    IsVisibleYear = true;
                }
                else if (selectedOption.Equals(nameof(ProductDB.Period.PE_AN)))
                {
                    IsVisibleMonth = false;
                    IsVisibleYear = true;
                }
                SelectionChangedEnum();
            }
        }
        public string Month
        {
            get => month;
            set
            {
                SetProperty(ref month, value);
                SelectionChangedEnum();
            }
        }

        public int Year
        {
            get => year;
            set
            {
                SetProperty(ref year, value);
                SelectionChangedEnum();
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

        public Category CurrentCategory
        {
            get => currentCategory;
            set => SetProperty(ref currentCategory, value);
        }

        public MonthYearVM CurrentMonthYearVM
        {
            get => currentMonthYearVM;
            set => SetProperty(ref currentMonthYearVM, value);
        }
        #endregion

        public async void InitializeCategoryList(int? chosenYear = null, int? chosenMonth = null)
        {
            List<CategoryListItemVM> updatedCategoryList = new List<CategoryListItemVM>();
            List<Category> categoryList = await categoryDB.GetListAsync();
            foreach (Category categ in categoryList)
            {
                updatedCategoryList.Add(new CategoryListItemVM(categ, ProductDB.StringToPeriod(SelectedOption), chosenYear, chosenMonth));
            }

            foreach (Category category in categoryList)
            {
                Console.WriteLine("CategoryID: " + category.CategoryId + " CategoryName: " + category.CategoryName +
                    " orderIndex: " + category.OrderIndex);
            }
            Categories = updatedCategoryList;
        }
        private void SelectionChangedEnum()
        {
            if (selectedOption.Equals(nameof(ProductDB.Period.TOTALE)))
            {

                IsVisibleMonth = false;
                IsVisibleYear = false;
                InitializeCategoryList();
            }
            else if (selectedOption.Equals(nameof(ProductDB.Period.PE_LUNA)))
            {
                int? currentYear = year;
                int? currentMonth = currentMonthYearVM.Months.IndexOf(month) + 1;
                InitializeCategoryList(currentYear, currentMonth);
            }
            else
            {
                InitializeCategoryList(Year);
            }

        }

        private void OnAddNewCategoryCommand()
        {
            Device.BeginInvokeOnMainThread(async () =>
           await Application.Current.MainPage.Navigation.PushAsync(new AddCategoryPage()));
        }

        private void OnInformationCommand()
        {
            Device.BeginInvokeOnMainThread(() =>
                Application.Current.MainPage.DisplayAlert("Instructiuni",
                                "Pentru a vedea lista de alimente filtrata dupa o categorie apasati pe cele 3 puncte", "OK"));
        }

        private void OnTapCategoryCommand(Category category)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (selectedOption.Equals(nameof(ProductDB.Period.TOTALE)))
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new FilterProductsByCategoryPage(category, ProductDB.Period.TOTALE));
                }
                else if (selectedOption.Equals(nameof(ProductDB.Period.PE_LUNA)))
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new FilterProductsByCategoryPage(category, ProductDB.Period.PE_LUNA, Year, Month));
                }
                else
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new FilterProductsByCategoryPage(category, ProductDB.Period.PE_AN, Year));
                }

            });
        }

        private async void OnDeleteSelectedCategoryCommand(Category category)
        {
            ProductDB productDB = new ProductDB();
            category.Products = await productDB.GetProductsWithTheGivenCategoryOrderByDateAsync(category.CategoryId);
            if (category.Products.Count != 0)
            {
                Device.BeginInvokeOnMainThread(() =>
                    Application.Current.MainPage.DisplayAlert("Alert", "Exista produse din categoria selectata! Intai stergeti produsele!"
                                                                        + category.CategoryName, "OK"));
                return;
            }
            await categoryDB.DeleteAsync(category);
            InitializeCategoryList();

        }

        public void OnUpdateCategoriesDataCommand()
        {
            SelectionChangedEnum();
        }
    }
}
