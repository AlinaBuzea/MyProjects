using EM.Data;
using EM.Models.Entity;
using EM.Pages;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EM.ViewModels.SubViewModels
{
    public class ProductsVM : BaseViewModel
    {
        #region Fields
        public string Id { get; set; }
        private string productName;
        private string quantity;
        private Shop aquisitionShop;
        private DateTime aquisitionDate;
        private string price;
        private Category prodCategory;
        private bool isMarked;

        private ProductDB productDB;
        List<Product> products;
        private Color backgroundColor;
        #endregion

        #region Commands
        public ICommand CancelCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand ModifyProductCommand { get; }
        #endregion

        private bool isRefreshing;
        public ICommand UpdateProductsCommand { get; }

        public ProductsVM()
        {
            CancelCommand = new Command(OnCancelCommand);
            SaveCommand = new Command(OnSaveCommand);
            ModifyProductCommand = new Command<string>(OnModifyProductCommand);

            productDB = new ProductDB();
            BackgroundColor = Color.White;

        }
        public ProductsVM(Product product)
        {
            productDB = new ProductDB();
            BackgroundColor = Color.White;

            Id = product.ProductId.ToString();
            ProductName = product.ProductName;
            InitializeAquisitionShop(product.AquisitionShopId);
            Quantity = product.Quantity;
            Price = product.Price.ToString();
            AquisitionDate = product.AquisitionDate;
            if (product.ProdCategoryId == 0)
            {
                ProdCategory = new Category();
                if (product.ProductCategory != null)
                {
                    ProdCategory.CategoryName = product.ProductCategory.CategoryName;
                    ProdCategory.OrderIndex = product.ProductCategory.OrderIndex;
                }
            }
            else
            {
                InitializeProdCategory(product.ProdCategoryId);
            }
            IsMarked = product.IsMarked;

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
        public Color BackgroundColor
        {
            get => backgroundColor;
            set => SetProperty(ref backgroundColor, value);
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
        #endregion

        private void InitializeProdCategory(int categId)
        {
            ProdCategory = Task.Run(async () => {
                CategoryDB categoryDB = new CategoryDB();
                return await categoryDB.GetAsync(categId);
            }).Result;
        }

        private void InitializeAquisitionShop(int shopId)
        {
            AquisitionShop = Task.Run(async () => {
                ShopDB shopDB = new ShopDB();
                return await shopDB.GetAsync(shopId);
            }).Result;
        }

        private void OnModifyProductCommand(string index)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new AddProductPage());
            });
        }

        private void OnCancelCommand()
        {
            Device.BeginInvokeOnMainThread(async () => await Application.Current.MainPage.Navigation.PopAsync());
        }

        private void OnSaveCommand()
        {
            if (string.IsNullOrWhiteSpace(ProductName) || ProdCategory == null ||
                string.IsNullOrWhiteSpace(Price))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.DisplayAlert("Alerta", "Denumirea, categoria si pretul sunt campuri obligatorii!", "OK");
                });
                return;
            }

            double resultPrice;
            if (Double.TryParse(Price, out resultPrice))
            {
                Products = Task.Run(async () =>
                {
                    Product newProduct = new Product();
                    newProduct.ProductName = ProductName;
                    newProduct.AquisitionShopId = AquisitionShop == null ? 0 : AquisitionShop.Id;
                    newProduct.Quantity = Quantity;
                    newProduct.Price = resultPrice;
                    newProduct.AquisitionDate = AquisitionDate;
                    newProduct.ProdCategoryId = ProdCategory.CategoryId;
                    newProduct.IsMarked = IsMarked;

                    await productDB.SaveAsync(newProduct);

                    return await productDB.GetListAsync();
                }).Result;

                foreach (Product product in Products)
                {
                    Console.WriteLine("ProductID: " + product.ProductId + " ProductName: " + product.ProductName +
                    " Category: " + product.ProdCategoryId + " ShopName: " + product.AquisitionShopId +
                    " Quantity: " + product.Quantity + " AquisitionDate: " + product.AquisitionDate.ToString() +
                    " Price: " + product.Price + " IsMarked: " + product.IsMarked);
                }

                ReinitializeFields();

            }
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
