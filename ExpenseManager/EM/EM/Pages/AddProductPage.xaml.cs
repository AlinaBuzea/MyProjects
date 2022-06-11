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
    public partial class AddProductPage : ContentPage
    {
        public AddProductPage()
        {
            InitializeComponent();
            BindingContext = new AddProductVM();
        }
        public AddProductPage(Product product = null)
        {
            InitializeComponent();
            BindingContext = new AddProductVM(product);
        }
    }
}