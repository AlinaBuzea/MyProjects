using EM.Data;
using EM.Models.Entity;
using EM.Pages;
using EM.Processing;
using EM.ViewModels.SubViewModels;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EM.ViewModels
{
    public class ReceiptInformationVM : BaseViewModel
    {
        #region Fields
        private const string errorHexColor = "#E68888";
        private string shopName;
        private string shopAddress;
        private DateTime aquisitionDate;
        List<ProductsVM> processedProducts;
        private Category currentProcessedProductCategory;

        private bool isRefreshing;
        private bool isSaveButtonEnabled;
        private List<Category> processedProductsCategories;
        public DateTime TodaysDate { get; }
        #endregion

        #region Commands
        public ICommand DeleteSelectedProcessedProductCommand { get; }
        public ICommand UpdateProcessedProductsCommand { get; }
        public ICommand SaveProductsCommand { get; }
        public ICommand CancelCommand { get; }
        #endregion

        #region Properties
        public string ShopName
        {
            get => shopName;
            set => SetProperty(ref shopName, value);
        }

        public string ShopAddress
        {
            get => shopAddress;
            set => SetProperty(ref shopAddress, value);
        }

        public DateTime AquisitionDate
        {
            get => aquisitionDate;
            set => SetProperty(ref aquisitionDate, value);
        }

        public List<ProductsVM> ProcessedProducts
        {
            get => processedProducts;
            set => SetProperty(ref processedProducts, value);
        }
        public Category CurrentProcessedProductCategory
        {
            get => currentProcessedProductCategory;
            set => SetProperty(ref currentProcessedProductCategory, value);
        }
        public bool IsRefreshing
        {
            get => isRefreshing;
            set => SetProperty(ref isRefreshing, value);
        }
        public bool IsSaveButtonEnabled
        {
            get => isSaveButtonEnabled;
            set => SetProperty(ref isSaveButtonEnabled, value);
        }
        #endregion

        public ReceiptInformationVM(List<Tuple<InformationExtractor.InformationType, string>> filteredInformationTuples)
        {
            processedProductsCategories = new List<Category>();
            ProcessedProducts = new List<ProductsVM>();
            AquisitionDate = DateTime.Today;
            TodaysDate = DateTime.Today;

            InitializeInformationFromInformationTuples(filteredInformationTuples);

            DeleteSelectedProcessedProductCommand = new Command<ProductsVM>(OnDeleteSelectedProcessedProductCommand);
            SaveProductsCommand = new Command(OnSaveProductsCommand);
            UpdateProcessedProductsCommand = new Command(OnUpdateProcessedProductsCommand);
            CancelCommand = new Command(OnCancelCommand);
            IsSaveButtonEnabled = true;
        }

        public ReceiptInformationVM()
        {
            Product prod = new Product();
            prod.ProdCategoryId = 1;
            prod.ProductName = "Lamai de casa din alea bune de zici ca sunt mandarine";
            prod.AquisitionDate = DateTime.Today;
            prod.Price = 10;
            prod.Quantity = "1kg";
            prod.AquisitionShopId = -1;
            Product prod1 = new Product();
            prod1.ProdCategoryId = 1;
            prod1.ProductName = "Portocale";
            prod1.AquisitionDate = DateTime.Today;
            prod1.Price = 7;
            prod1.Quantity = "";
            prod1.AquisitionShopId = -1;

            ProcessedProducts = new List<ProductsVM>()
            {
                new ProductsVM(prod),
                new ProductsVM(prod1)
            };

            ShopName = "LIDL";
            ShopAddress = "Bd. Victoriei, Nr.12";
            AquisitionDate = DateTime.Today;


            TodaysDate = DateTime.Today;

            DeleteSelectedProcessedProductCommand = new Command<ProductsVM>(OnDeleteSelectedProcessedProductCommand);
            SaveProductsCommand = new Command(OnSaveProductsCommand);
        }

        public void OnDeleteSelectedProcessedProductCommand(ProductsVM currentProcessedProduct)
        {
            ProcessedProducts.Remove(currentProcessedProduct);
            List<ProductsVM> list = ProcessedProducts;
            ProcessedProducts = new List<ProductsVM>(list); 
        }

        private void OnUpdateProcessedProductsCommand()
        {
            List<ProductsVM> list = ProcessedProducts;
            ProcessedProducts = new List<ProductsVM>(list);
            IsRefreshing = false;
        }

        public async void OnCancelCommand()
        {
            await Application.Current.MainPage.Navigation.PopAsync();//ReceiptInformationPage
            await Application.Current.MainPage.Navigation.PopAsync();//ShowReceiptPage
        }
        private async void OnSaveProductsCommand()
        {
            if (IsSaveButtonEnabled)
            {
                IsSaveButtonEnabled = false;
                Console.WriteLine("ShopName: " + ShopName + "\nShopAddress: " + ShopAddress + "\nAquisitionDate: " + AquisitionDate.ToString());
                foreach (ProductsVM product in ProcessedProducts)
                {
                    Console.WriteLine(" ProductName: " + product.ProductName +
                        " Category: " + product.ProdCategory.CategoryName +
                        " Quantity: " + product.Quantity +
                        " Price: " + product.Price);
                }

                ShopDB shopDB = new ShopDB();
                int shopId = await shopDB.GetIdByNameAndAddressAsync(ShopName, ShopAddress); 
                if (shopId == 0)
                {
                    await shopDB.SaveAsync(new Shop() { ShopName = this.ShopName, ShopAddress = this.ShopAddress });
                    shopId = await shopDB.GetIdByNameAndAddressAsync(ShopName, ShopAddress);
                }
                await Task.Run(async () =>
                {
                    List<Shop> _list = await shopDB.GetListAsync();
                    foreach (Shop shop in _list)
                    {
                        Console.WriteLine("shopId = " + shop.Id + " shopName = " + shop.ShopName + " shopAddress = " + shop.ShopAddress);
                    }
                });

                CategoryDB categoryDB = new CategoryDB();
                ProductDB productDB = new ProductDB();
                if (await HasWrongInputs())
                {
                    IsSaveButtonEnabled = true;
                    return;
                }

                foreach (ProductsVM productVM in ProcessedProducts)
                {
                    int currentCategId = await categoryDB.GetIdByNameAsync(productVM.ProdCategory.CategoryName);
                    if (currentCategId == 0)
                    {
                        await categoryDB.SaveAsync(productVM.ProdCategory);
                        currentCategId = await categoryDB.GetIdByNameAsync(productVM.ProdCategory.CategoryName);
                    }

                    Product product = new Product();
                    product.ProductName = productVM.ProductName;
                    product.ProdCategoryId = currentCategId;
                    try
                    {
                        product.Price = Double.Parse(productVM.Price, NumberStyles.AllowDecimalPoint);
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    productVM.BackgroundColor = Color.White;
                    product.Quantity = productVM.Quantity;
                    product.AquisitionDate = AquisitionDate;
                    product.AquisitionShopId = shopId;
                    product.IsMarked = false;

                    await productDB.SaveAsync(product);
                }

                //await Task.Run(async () =>
                //{
                //    List<Product> products = await productDB.GetListAsync();
                //    foreach (Product product in products)
                //    {
                //        Console.WriteLine("ProductID: " + product.ProductId + " ProductName: " + product.ProductName +
                //            " Category: " + product.ProdCategoryId + " ShopName: " + product.AquisitionShopId +
                //            " Quantity: " + product.Quantity + " AquisitionDate: " + product.AquisitionDate.ToString() +
                //            " Price: " + product.Price + " IsMarked: " + product.IsMarked);
                //    }
                //});

                await Application.Current.MainPage.Navigation.PopAsync();//ReceiptInformationPage
                await Application.Current.MainPage.Navigation.PopAsync();//ShowReceiptPage
                await Application.Current.MainPage.Navigation.PushAsync(new CategoryListPage());
            }
        }

        private void InitializeInformationFromInformationTuples(List<Tuple<InformationExtractor.InformationType, string>> filteredInformationTuples)
        {
            Product currentProduct = new Product();
            ShopName = "";
            ShopAddress = "";
            foreach (Tuple<InformationExtractor.InformationType, string> tuple in filteredInformationTuples)
            {
                switch (tuple.Item1)
                {
                    case InformationExtractor.InformationType.Other:
                        break;
                    case InformationExtractor.InformationType.ShopName:
                        ShopName += tuple.Item2 + " ";
                        break;
                    case InformationExtractor.InformationType.Address:
                        ShopAddress += tuple.Item2 + " ";
                        break;
                    case InformationExtractor.InformationType.ProductName:
                        if (currentProduct.ProductName != null)
                        {
                            ProcessedProducts.Add(new ProductsVM(currentProduct));
                        }
                        currentProduct = new Product();
                        currentProduct.ProductName = tuple.Item2;
                        break;
                    case InformationExtractor.InformationType.Category:
                        processedProductsCategories.Add(new Category() { CategoryName = tuple.Item2 });
                        break;
                    case InformationExtractor.InformationType.Price:
                        if (tuple.Item2.Contains(","))
                        {
                            string newStringPrice = tuple.Item2.Replace(',', '.');
                            currentProduct.Price = Double.Parse(newStringPrice, CultureInfo.InvariantCulture);
                            break;
                        }
                        currentProduct.Price = Double.Parse(tuple.Item2, CultureInfo.InvariantCulture);
                        break;
                    case InformationExtractor.InformationType.Quantity:
                        currentProduct.Quantity = tuple.Item2;
                        break;
                    case InformationExtractor.InformationType.Date:
                        try
                        {
                            DateTime date = TodaysDate;
                            DateTimeStyles styles = DateTimeStyles.None;
                            foreach (CultureInfo cInfo in CultureInfo.GetCultures(CultureTypes.AllCultures))
                            {
                                DateTime.TryParse(tuple.Item2, cInfo, styles, out date);
                            }
                            AquisitionDate = date;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Exception while try parse datetime: " + e.Message);
                            AquisitionDate = TodaysDate;
                        }
                        break;
                }
            }

            ProcessedProducts.Add(new ProductsVM(currentProduct));
            //Console.WriteLine("ShopName: " + ShopName + "\nShopAddress: " + ShopAddress + "\nAquisitionDate: " + AquisitionDate.ToString());
            //foreach (ProductsVM product in ProcessedProducts)
            //{
            //    Console.WriteLine(" ProductName: " + product.ProductName +
            //        " Category: " + product.ProdCategory.CategoryName +
            //        " Quantity: " + product.Quantity +
            //        " Price: " + product.Price);
            //}

            UpdateProcessedProducts();
            //Task.Run(() =>
            //{
            //    Console.WriteLine("ShopName: " + ShopName + "\nShopAddress: " + ShopAddress + "\nAquisitionDate: " + AquisitionDate.ToString());
            //    foreach (ProductsVM product in ProcessedProducts)
            //    {
            //        Console.WriteLine(" ProductName: " + product.ProductName +
            //            " Category: " + product.ProdCategory.CategoryName +
            //            " Quantity: " + product.Quantity +
            //            " Price: " + product.Price);
            //    }
            //});
        }
        private async Task<bool> HasWrongInputs()
        {
            bool noCategory = false;
            bool nameFieldIsNotCompleted = false;
            bool priceFieldIsZeroOrNegative = false;
            bool isNotNumber = false;
            bool isNumberInAWrongFormat = false;
            foreach (ProductsVM productVM in ProcessedProducts)
            {
                if(string.IsNullOrWhiteSpace(productVM.ProductName) || productVM.ProductName.Equals(""))
                {
                    nameFieldIsNotCompleted = true;
                    productVM.BackgroundColor = Color.FromHex(errorHexColor);
                }
                if (productVM.ProdCategory == null || (string.IsNullOrWhiteSpace(productVM.ProdCategory.CategoryName) ||
                    productVM.ProdCategory.CategoryName.Equals("")))
                {
                    noCategory = true;
                    productVM.BackgroundColor = Color.FromHex(errorHexColor);
                }
                double validPrice = -1;
                try
                {

                    validPrice = double.Parse(productVM.Price, CultureInfo.InvariantCulture);
                     _= Double.Parse(productVM.Price, NumberStyles.AllowDecimalPoint);
                    productVM.BackgroundColor = (string.IsNullOrWhiteSpace(productVM.ProductName) || productVM.ProductName.Equals("") ||
                                            productVM.ProdCategory == null || (string.IsNullOrWhiteSpace(productVM.ProdCategory.CategoryName) ||
                                                 productVM.ProdCategory.CategoryName.Equals(""))) ? 
                                                Color.FromHex(errorHexColor): Color.White;
                }
                catch (Exception)
                {
                    productVM.BackgroundColor = Color.FromHex(errorHexColor);
                    if (validPrice == -1)
                    {
                        isNumberInAWrongFormat = true;
                    }
                    else
                    {
                        isNotNumber = true;
                    }
                }
                if (validPrice <= 0)
                {
                    priceFieldIsZeroOrNegative = true;
                    productVM.BackgroundColor = Color.FromHex(errorHexColor);
                }
            }

            if (nameFieldIsNotCompleted)
            {
                await Application.Current.MainPage.DisplayAlert("Denumire - Date invalide!", "Fiecare produs trebuie sa aiba o denumire", "OK");
                return true;
            }
            if (noCategory)
            {
                await Application.Current.MainPage.DisplayAlert("Categorie - Date invalide!", "Fiecare produs trebuie sa aiba o categorie", "OK");
                return true;
            }
            if (isNotNumber)
            {
                await Application.Current.MainPage.DisplayAlert("Pret - Date invalide!", "Pretul este camp numeric! Introduceti un numar!", "OK");
                return true;
            }
            if (isNumberInAWrongFormat)
            {
                await Application.Current.MainPage.DisplayAlert("Pret - Date invalide!", "Zecimalele se despart de intreg prin virgula! Inlocuiti .(punct) cu ,(virgula) si reciproc!", "OK");
                return true;
            }
            if (priceFieldIsZeroOrNegative)
            {
                await Application.Current.MainPage.DisplayAlert("Pret - Date invalide!", "Pretul unui produs NU poate fi mai mic sau egal cu 0!", "OK");
                return true;
            }

            return false;
        }
        private void UpdateProcessedProducts()
        {
            for (int index = 0; index < processedProductsCategories.Count; index++)
            {
                ProcessedProducts[index].ProdCategory = processedProductsCategories[index];
            }
        }
    }
}
