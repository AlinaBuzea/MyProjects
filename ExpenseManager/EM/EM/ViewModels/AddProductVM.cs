using EM.Data;
using EM.Models;
using EM.Models.Entity;
using EM.Pages;
using EM.ViewModels.SubViewModels;
using MvvmHelpers;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EM.ViewModels
{
    public class AddProductVM : BaseViewModel
    {
        #region Fields
        private string Id;
        private string productName;
        private string quantity;
        private Shop aquisitionShop;
        private DateTime aquisitionDate;
        private Category prodCategory;
        private string price;
        private bool isMarked;

        private ProductDB productDB;
        private BudgetDB budgetDB;
        List<Product> products;
        List<Product> categoryProducts;
        List<Category> categories;
        List<Shop> shops;

        private bool isRefreshing;
        private bool updateProduct;
        public DateTime TodaysDate { get; }
        #endregion

        #region Commands
        public ICommand CancelCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand UpdateProductsCommand { get; }
        public ICommand AddNewCategoryCommand { get; }
        public ICommand AddNewShopCommand { get; }
        public ICommand UpdateCategoriesAndShopsDataCommand { get; }
        #endregion

        public AddProductVM()
        {
            TodaysDate = DateTime.Today;
            AquisitionDate = TodaysDate;
            CancelCommand = new Command(OnCancelCommand);
            SaveCommand = new Command(OnSaveCommand);
            UpdateProductsCommand = new Command<Category>(OnUpdateProductsCommand);
            AddNewCategoryCommand = new Command(OnAddNewCategoryCommand);
            AddNewShopCommand = new Command(OnAddNewShopCommand);
            UpdateCategoriesAndShopsDataCommand = new Command(OnUpdateCategoriesAndShopsDataCommand);
            productDB = new ProductDB();
            budgetDB = new BudgetDB();
            InitializeCategoriesList();
            InitializeShopsList();
            InitializeCategoryProductsList();

            updateProduct = false;
        }

        public AddProductVM(Product product)
        {
            TodaysDate = DateTime.Today;
            AquisitionDate = TodaysDate;
            CancelCommand = new Command(OnCancelCommand);
            SaveCommand = new Command(OnSaveCommand);
            UpdateProductsCommand = new Command<Category>(OnUpdateProductsCommand);
            AddNewCategoryCommand = new Command(OnAddNewCategoryCommand);
            AddNewShopCommand = new Command(OnAddNewShopCommand);
            UpdateCategoriesAndShopsDataCommand = new Command(OnUpdateCategoriesAndShopsDataCommand);
            productDB = new ProductDB();
            InitializeCategoriesList();
            InitializeShopsList();
            InitializeCategoryProductsList();

            updateProduct = true;
            InitializeFields(product);
        }

        #region Properties
        public string ProductName
        {
            get => productName;
            set => SetProperty(ref productName, value);
        }

        public string Quantity
        {
            get => quantity;
            set => SetProperty(ref quantity, value);
        }

        public Shop AquisitionShop
        {
            get => aquisitionShop;
            set => SetProperty(ref aquisitionShop, value);
        }

        public DateTime AquisitionDate
        {
            get => aquisitionDate;
            set => SetProperty(ref aquisitionDate, value);
        }

        public string Price
        {
            get => price;
            set => SetProperty(ref price, value);
        }

        public Category ProdCategory
        {
            get => prodCategory;
            set => SetProperty(ref prodCategory, value);
        }

        public bool IsMarked
        {
            get => isMarked;
            set => SetProperty(ref isMarked, value);
        }

        public bool IsRefreshing
        {
            get => isRefreshing;
            set => SetProperty(ref isRefreshing, value);
        }
        public List<Product> Products
        {
            get => products;
            set => SetProperty(ref products, value);
        }

        public List<Product> CategoryProducts
        {
            get => categoryProducts;
            set => SetProperty(ref categoryProducts, value);
        }

        public List<Category> Categories
        {
            get => categories;
            set => SetProperty(ref categories, value);
        }

        public List<Shop> Shops
        {
            get => shops;
            set => SetProperty(ref shops, value);
        }
        #endregion

        private void OnCancelCommand()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });
        }

        private async void OnSaveCommand()
        {
            if (string.IsNullOrWhiteSpace(ProductName) || prodCategory == null ||
                string.IsNullOrWhiteSpace(Price))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.DisplayAlert("Alerta", "Denumirea, categoria si pretul sunt campuri obligatorii!", "OK");
                });
                return;
            }

            double resultPrice;

            if (!Double.TryParse(Price, out resultPrice))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.DisplayAlert("Alerta", "Pretul este camp numeric!", "OK");
                });
                return;
            }

            
            Product newProduct = new Product();
            if (updateProduct)
            {
                newProduct.ProductId = Int32.Parse(Id);
            }

            newProduct.ProductName = ProductName;
            newProduct.AquisitionShopId = AquisitionShop == null ? 0 : AquisitionShop.Id;
            newProduct.Quantity = Quantity == null ? null : Quantity.ToUpper();
            newProduct.Price = resultPrice;
            newProduct.AquisitionDate = AquisitionDate;
            newProduct.ProdCategoryId = ProdCategory.CategoryId;
            newProduct.IsMarked = IsMarked;

            await productDB.SaveAsync(newProduct);
            Products = await productDB.GetListOrderedByDateAsync();

            _  = await UpdateAndVerifyBudget();
            //Task.Run(() =>
            //{
            //    foreach (Product product in Products)
            //    {
            //        Console.WriteLine("ProductID: " + product.ProductId + " ProductName: " + product.ProductName +
            //            " Category: " + product.ProdCategoryId + " ShopName: " + product.AquisitionShopId +
            //            " Quantity: " + product.Quantity + " AquisitionDate: " + product.AquisitionDate.ToString() +
            //            " Price: " + product.Price + " IsMarked: " + product.IsMarked);
            //    }
            //}).Wait();
            ReinitializeFields();
            UpdateFilteredProductsByCategoryPage();
            UpdateListCategoryPage();
        }

        private async Task<int> UpdateAndVerifyBudget()
        {
            Budget currentBudget = await budgetDB.GetByCategoryMonthYearAsync(ProdCategory.CategoryId,
                                                                   ((MonthYearVM.MonthEnum)AquisitionDate.Month).ToString(), AquisitionDate.Year);
            Category currentCategory = new Category() { CategoryId = ProdCategory.CategoryId, CategoryName = ProdCategory.CategoryName };
            if (currentBudget == null) 
            {
                await CreateBudgetEntry();
                currentBudget = await budgetDB.GetByCategoryMonthYearAsync(currentCategory.CategoryId,
                                                                   ((MonthYearVM.MonthEnum)AquisitionDate.Month).ToString(), AquisitionDate.Year);
            }
                
            if (currentBudget != null)
            {
                double newPrice = await productDB.GetTotalPriceFromProductsWithTheGivenCategoryAndBoughtInMonthYearAsync(
                                                                      currentCategory.CategoryId,
                                                                     AquisitionDate.Year, AquisitionDate.Month);
                currentBudget.CurrentValue = newPrice;
                if (currentBudget.AlocatedBudget != 0 && currentBudget.AlocatedBudget <= currentBudget.CurrentValue)
                {
                    await NotificationCenter.Current.Show(NotificationClass.ShowBudgetExceededNotification(currentCategory.CategoryName, 
                                                           ((MonthYearVM.MonthEnum)AquisitionDate.Month).ToString(), AquisitionDate.Year));
                    return await budgetDB.SaveAsync(currentBudget);
                }

                if (currentBudget.LimitNotificationValue != 0 && currentBudget.AlocatedBudget - currentBudget.CurrentValue <= currentBudget.LimitNotificationValue)
                {
                    await NotificationCenter.Current.Show(NotificationClass.ShowLimitBudgetExceededNotification(currentCategory.CategoryName,
                                                          ((MonthYearVM.MonthEnum)AquisitionDate.Month).ToString(), AquisitionDate.Year));
                    return await budgetDB.SaveAsync(currentBudget);
                }
                return await budgetDB.SaveAsync(currentBudget);
            }
            return 0;
        }

        private async Task CreateBudgetEntry()
        {
            CategoryDB categoryDB = new CategoryDB();
            List<Category> categories = await categoryDB.GetListAsync();
            foreach(Category category in categories)
            {
                Budget newBudget = new Budget();
                newBudget.CategoryId = category.CategoryId;
                newBudget.CurrentValue = await productDB.GetTotalPriceFromProductsWithTheGivenCategoryAndBoughtInMonthYearAsync(newBudget.CategoryId, AquisitionDate.Year, AquisitionDate.Month);
                newBudget.AlocatedBudget = 0;
                newBudget.LimitNotificationValue = 0;
                newBudget.Month = ((MonthYearVM.MonthEnum)AquisitionDate.Month).ToString();
                newBudget.Year = AquisitionDate.Year;

                await budgetDB.SaveAsync(newBudget);
            }
        }

        private void UpdateFilteredProductsByCategoryPage()
        {
            int navPgNo = Application.Current.MainPage.Navigation.NavigationStack.Count;
            var secondLastPage = navPgNo > 2 ? Application.Current.MainPage.Navigation.NavigationStack[navPgNo - 2] : null;
            if (secondLastPage != null)
            {
                if (secondLastPage.GetType().Name.Equals(nameof(FilterProductsByCategoryPage)))
                {
                    ProductsPerCategoryVM x = (ProductsPerCategoryVM)secondLastPage.BindingContext;
                    x.OnUpdateProductsCommand();
                }
            }
        }

        private void UpdateListCategoryPage()
        {
            int navPgNo = Application.Current.MainPage.Navigation.NavigationStack.Count;
            var secondLastPage = navPgNo > 3 ? Application.Current.MainPage.Navigation.NavigationStack[navPgNo - 3] : null;
            if (secondLastPage != null)
            {
                if (secondLastPage.GetType().Name.Equals(nameof(CategoryListPage)))
                {
                    ListCategoriesVM x = (ListCategoriesVM)secondLastPage.BindingContext;
                    x.OnUpdateCategoriesDataCommand();
                }
            }
        }
        private async void AddNewProductToDatabase(double resultPrice)
        {
            Product newProduct = new Product();
            newProduct.ProductName = ProductName;
            newProduct.AquisitionShopId = AquisitionShop == null ? 0 : AquisitionShop.Id;
            newProduct.Quantity = Quantity.ToUpper();
            newProduct.Price = resultPrice;
            newProduct.AquisitionDate = AquisitionDate;
            newProduct.ProdCategoryId = ProdCategory.CategoryId;
            newProduct.IsMarked = IsMarked;

            await productDB.SaveAsync(newProduct);
        }

        private void OnAddNewCategoryCommand()
        {
            Device.BeginInvokeOnMainThread(async () => {
                await Application.Current.MainPage.Navigation.PushAsync(new AddCategoryPage());
                InitializeCategoriesList();
            });
        }

        private void OnAddNewShopCommand()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new AddShopPage());
                InitializeShopsList();
            });
        }

        private void OnUpdateProductsCommand(Category referencedCategory)
        {
            InitializeCategoryProductsList(referencedCategory);
        }

        private void OnUpdateCategoriesAndShopsDataCommand()
        {
            InitializeCategoriesList();
            InitializeShopsList();
            IsRefreshing = false;
        }

        public void InitializeCategoryProductsList(Category referencedCategory = null)
        {

            Products = Task.Run(async () => await productDB.GetListOrderedByDateAsync()).Result; //.GetAwaiter().GetResult();
            if (referencedCategory == null)
            {
                CategoryProducts = Products;
                return;
            }

            CategoryProducts = Task.Run(async () => await productDB.GetProductsWithTheGivenCategoryOrderByDateAsync(referencedCategory.CategoryId)).Result;

        }

        public void InitializeCategoriesList()
        {
            Categories = Task.Run(async () =>
            {
                CategoryDB categoryDB = new CategoryDB();
                return await categoryDB.GetListAsync();
            }).Result;
        }

        public void InitializeShopsList()
        {
            Shops = Task.Run(async () =>
            {
                ShopDB shopDB = new ShopDB();
                return await shopDB.GetListAsync();
            }).Result;
        }

        private void InitializeFields(Product product)
        {
            Id = product.ProductId.ToString();
            ProductName = product.ProductName;
            AquisitionShop = Task.Run(async () =>
            {
                ShopDB shopDB = new ShopDB();
                return await shopDB.GetAsync(product.AquisitionShopId);
            }).Result;
            Quantity = product.Quantity;
            Price = product.Price.ToString();
            AquisitionDate = product.AquisitionDate;
            ProdCategory = Task.Run(async () =>
            {
                CategoryDB categoryDB = new CategoryDB();
                return await categoryDB.GetAsync(product.ProdCategoryId);
            }).Result;
            IsMarked = product.IsMarked;
        }

        private void ReinitializeFields()
        {
            ProductName = "";
            AquisitionShop = null;
            Quantity = "";
            Price = null;
            AquisitionDate = DateTime.Today;
            ProdCategory = null;
            IsMarked = false;
        }
    }
}
