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
    public partial class BudgetMenuPage : ContentPage
    {
        public ICommand OpenIncomesCommand => new Command(async () =>
        {
            await Application.Current.MainPage.Navigation.PushAsync(new IncomesPage());
        });

        public ICommand OpenBudgetPlanerCommand => new Command(async () =>
        {
            await Application.Current.MainPage.Navigation.PushAsync(new BudgetPlanerPage());
        });

        public ICommand OpenActualStateCommand => new Command(async () =>
        {
            await Application.Current.MainPage.Navigation.PushAsync(new BudgetActualStatePage());
        });

        public BudgetMenuPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
    }
}