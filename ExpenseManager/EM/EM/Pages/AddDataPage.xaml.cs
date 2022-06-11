using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EM.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddDataPage : ContentPage
    {
        public ICommand OpenAddShopCommand => new Command(async () =>
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AddShopPage());
        });

        public ICommand OpenAddProductCommand => new Command(async () =>
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AddProductPage());
        });

        public ICommand OpenAddCategoryCommand => new Command(async () =>
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AddCategoryPage());
        });

        public ICommand OpenAllBoughtProductsCommand => new Command(async () =>
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AllBoughtProductsPage());
        });

        public ICommand AddReceiptCommand => new Command(async () =>
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ImportReceiptPage());
        });

        public AddDataPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
    }
}