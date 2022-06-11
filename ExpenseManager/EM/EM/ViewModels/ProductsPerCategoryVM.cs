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
    public class ProductsPerCategoryVM : BaseViewModel
    {
        #region Fields
        private Category prodCategory;
        //private bool isMarked;
        private ProductDB productDB;

        private string month;
        private int year;
        private MonthYearVM currentMonthYearVM;
        private bool isVisibleMonth;
        private bool isVisibleYear;
        private string selectedPeriod;
        public List<string> PeriodOption { get; }

        private string pageCategoryName;
        List<ProductsVM> categoryProducts;
        List<Product> products;
        private bool isRefreshing;
        #endregion

        #region Commands
        public ICommand DeleteProductCommand { get; }
        public ICommand ModifyProductCommand { get; }
        public ICommand ModifyIsMarkedProductCommand { get; }
        public ICommand AddNewProductCommand { get; }
        public ICommand InformationCommand { get; }
        #endregion

        public ProductsPerCategoryVM(Category referencedCategory, ProductDB.Period currentPeriod, int? year = null, string month = null)
        {
            DeleteProductCommand = new Command<string>(OnDeleteProductCommand);
            ModifyProductCommand = new Command<string>(OnModifyProductCommand);
            ModifyIsMarkedProductCommand = new Command<string>(OnModifyIsMarkedProductCommand);
            AddNewProductCommand = new Command(OnAddNewProductCommand);
            InformationCommand = new Command(OnInformationCommand);
            productDB = new ProductDB();
            PageCategoryName = referencedCategory.CategoryName.ToUpper();
            ProdCategory = referencedCategory;
            currentMonthYearVM = new MonthYearVM();
            PeriodOption = new List<string>() { nameof(ProductDB.Period.TOTALE), nameof(ProductDB.Period.PE_LUNA), nameof(ProductDB.Period.PE_AN) };
            SelectedPeriod = currentPeriod.ToString();
            Month = month != null ? month : currentMonthYearVM.Months[DateTime.Today.Month - 1];
            Year = year != null ? year.Value : DateTime.Today.Year;
            InitializeCategoryProductsList();
        }

        #region Properties
        public Category ProdCategory
        {
            get => prodCategory;
            set => SetProperty(ref prodCategory, value);
        }

        //public bool IsMarked
        //{
        //    get => isMarked;
        //    set => SetProperty(ref isMarked, value);
        //}

        public string Month
        {
            get => month;
            set
            {
                SetProperty(ref month, value);
                SelectionChangedEnum(ProdCategory);
            }
        }

        public int Year
        {
            get => year;
            set
            {
                SetProperty(ref year, value);
                SelectionChangedEnum(ProdCategory);
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
        public MonthYearVM CurrentMonthYearVM
        {
            get => currentMonthYearVM;
            set => SetProperty(ref currentMonthYearVM, value);
        }

        public string SelectedPeriod
        {
            get => selectedPeriod;
            set
            {
                SetProperty(ref selectedPeriod, value);
                if (selectedPeriod.Equals(nameof(ProductDB.Period.PE_LUNA)))
                {
                    IsVisibleMonth = true;
                    IsVisibleYear = true;
                }
                else if (selectedPeriod.Equals(nameof(ProductDB.Period.PE_AN)))
                {
                    IsVisibleMonth = false;
                    IsVisibleYear = true;
                }
                InitializeCategoryProductsList(ProdCategory);
            }
        }

        public bool IsRefreshing
        {
            get => isRefreshing;
            set => SetProperty(ref isRefreshing, value);
        }

        public List<ProductsVM> CategoryProducts
        {
            get => categoryProducts;
            set => SetProperty(ref categoryProducts, value);
        }

        public string PageCategoryName
        {
            get => pageCategoryName;
            set => SetProperty(ref pageCategoryName, value);
        }
        #endregion

        private void OnAddNewProductCommand()
        {
            Device.BeginInvokeOnMainThread(async () => await Application.Current.MainPage.Navigation.PushAsync(new AddProductPage()));
        }

        private void OnInformationCommand()
        {
            Device.BeginInvokeOnMainThread(() =>
                Application.Current.MainPage.DisplayAlert("Instructiuni", "Pentru a modifica un produs, apasati pe cele 3 puncte", "OK"));
        }

        private void OnModifyIsMarkedProductCommand(string productId)
        {
            Product product = Task.Run(async () => await productDB.GetAsync(Int32.Parse(productId))).Result;
            product.IsMarked = !product.IsMarked;
            Task.Run(async () => await productDB.SaveAsync(product)).Wait();
            PageCategoryName = ProdCategory.CategoryName.ToUpper();
        }

        private void OnModifyProductCommand(string productId)
        {
            Device.BeginInvokeOnMainThread(async () => {
                Product product = Task.Run(async () => await productDB.GetAsync(Int32.Parse(productId))).Result;
                //_= Application.Current.MainPage.DisplayAlert("Alert", "View Product: " + index, "OK");
                await Application.Current.MainPage.Navigation.PushAsync(new AddProductPage(product));
            });
        }

        public void OnDeleteProductCommand(string productId)
        {
            Task.Run(async () =>
            {
                List<Product> productList = await productDB.GetListAsync();
                foreach (Product product in productList)
                {
                    if (product.ProductId == Int32.Parse(productId))
                    {
                        await productDB.DeleteAsync(product);
                        break;
                    }
                }
                InitializeCategoryProductsList(ProdCategory);
            });

            UpdateCategoriesDataFromListCategoriesPage();
        }

        private void UpdateCategoriesDataFromListCategoriesPage()
        {
            int navPgNo = Application.Current.MainPage.Navigation.NavigationStack.Count;
            var secondLastPage = Application.Current.MainPage.Navigation.NavigationStack[navPgNo - 2];
            if (secondLastPage != null)
            {
                ListCategoriesVM x = (ListCategoriesVM)secondLastPage.BindingContext;
                x.OnUpdateCategoriesDataCommand();
            }
        }

        private void SelectionChangedEnum(Category referencedCategory)
        {
            if (selectedPeriod.Equals(nameof(ProductDB.Period.TOTALE)))
            {
                IsVisibleMonth = false;
                IsVisibleYear = false;
                products = Task.Run(async () => await productDB.GetProductsWithTheGivenCategoryOrderByDateAsync(referencedCategory.CategoryId)).Result;
            }
            else if (selectedPeriod.Equals(nameof(ProductDB.Period.PE_LUNA)))
            {
                int? currentMonth = currentMonthYearVM.Months.IndexOf(month) + 1;
                products = Task.Run(async () => await productDB.GetProductsWithTheGivenCategoryAndBoughtInMonthYearAsync(referencedCategory.CategoryId, year, currentMonth)).Result;
            }
            else
            {
                products = Task.Run(async () => await productDB.GetProductsWithTheGivenCategoryAndBoughtInMonthYearAsync(referencedCategory.CategoryId, year)).Result;
            }

            if (products.Count == 0)
            {
                CategoryProducts = null;
                return;
            }
            FormCategoryProductList();
        }

        public void InitializeCategoryProductsList(Category referencedCategory = null)
        {
            products = new List<Product>();
            CategoryProducts = new List<ProductsVM>();

            if (referencedCategory == null)
            {
                CategoryProducts = null;
                return;
            }

            if (referencedCategory.Equals("All"))
            {
                products = Task.Run(async () => await productDB.GetListAsync()).Result;
                FormCategoryProductList();
                return;
            }

            SelectionChangedEnum(referencedCategory);
        }

        private void FormCategoryProductList()
        {
            List<ProductsVM> list = new List<ProductsVM>();

            foreach (Product currentProd in products)
            {
                Console.WriteLine("ProductID: " + currentProd.ProductId + " ProductName: " + currentProd.ProductName +
                        " Category: " + currentProd.ProdCategoryId + " ShopName: " + currentProd.AquisitionShopId +
                        " Quantity: " + currentProd.Quantity + " AquisitionDate: " + currentProd.AquisitionDate.Date.ToString() +
                        " Price: " + currentProd.Price + " IsMarked: " + currentProd.IsMarked);

                list.Add(new ProductsVM(currentProd));
            }

            CategoryProducts = new List<ProductsVM>(list);
        }

        public void OnUpdateProductsCommand()
        {
            InitializeCategoryProductsList(ProdCategory);
        }
    }
}
