using EM.Data;
using EM.Models.Entity;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EM.ViewModels.SubViewModels
{
    public class CategoryListItemVM : BaseViewModel
    {
        #region Fields
        public int CurrentCategoryId { get; set; }
        private Category currentCategory;
        private string periodicalPayments;
        public static string SelectedCategory { get; set; }
        #endregion

        public CategoryListItemVM() { }

        public CategoryListItemVM(Category categ, ProductDB.Period period, int? year = null, int? month = null)
        {
            SelectedCategory = null;
            CurrentCategory = categ;
            CalculatePeriodicalPayments(period, year, month);
        }

        #region Properties
        public Category CurrentCategory
        {
            get => currentCategory;
            set => SetProperty(ref currentCategory, value);
        }

        public string PeriodicalPayments
        {
            get => periodicalPayments;
            set => SetProperty(ref periodicalPayments, value);
        }
        #endregion

        public void CalculatePeriodicalPayments(ProductDB.Period period, int? year = null, int? month = null)
        {
            ProductDB productDB = new ProductDB();
            List<Product> Products = new List<Product>();
            double sum = 0;
            switch (period)
            {
                case ProductDB.Period.PE_LUNA:
                    Products = Task.Run(async () =>
                                await productDB.GetProductsWithTheGivenCategoryAndBoughtInMonthYearAsync(CurrentCategory.CategoryId, year.Value, month)
                                ).Result;
                    break;
                case ProductDB.Period.PE_AN:
                    Products = Task.Run(async () =>
                                await productDB.GetProductsWithTheGivenCategoryAndBoughtInMonthYearAsync(CurrentCategory.CategoryId, year.Value)
                                ).Result;
                    break;
                case ProductDB.Period.TOTALE:
                    Products = Task.Run(async () =>
                               await productDB.GetProductsWithTheGivenCategoryOrderByDateAsync(CurrentCategory.CategoryId)
                               ).Result;
                    break;
            }

            foreach (Product product in Products)
            {
                Console.WriteLine("ProductID: " + product.ProductId + " ProductName: " + product.ProductName +
                    " Category: " + product.ProdCategoryId + " ShopName: " + product.AquisitionShopId +
                    " Quantity: " + product.Quantity + " AquisitionDate: " + product.AquisitionDate.Date.ToString() +
                    " Price: " + product.Price + " IsMarked: " + product.IsMarked);
                sum += product.Price;
            }
            PeriodicalPayments = sum.ToString();
        }
    }
}
