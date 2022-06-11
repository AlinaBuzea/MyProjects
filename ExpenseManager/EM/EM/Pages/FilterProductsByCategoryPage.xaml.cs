using EM.Data;
using EM.Models.Entity;
using EM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EM.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterProductsByCategoryPage : ContentPage
    {
        public FilterProductsByCategoryPage(Category currentCategory, ProductDB.Period currentPeriod, int? year = null, string month = null)
        {
            InitializeComponent();
            this.BindingContext = new ProductsPerCategoryVM(currentCategory, currentPeriod, year, month);
        }
    }
}