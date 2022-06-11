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
    public partial class MainMenuPage : ContentPage
    {
        public ICommand OpenExpensesCommand => new Command(async () =>
        {
            await Shell.Current.GoToAsync(nameof(CategoryListPage));
        });

        public ICommand OpenAddDataCommand => new Command(async () =>
        {
            await Shell.Current.GoToAsync(nameof(AddDataPage));
        });

        public ICommand OpenBudgetMenuCommand => new Command(async () =>
        {
            await Shell.Current.GoToAsync(nameof(BudgetMenuPage));
        });

        public ICommand OpenSavingsCommand => new Command(async () =>
        {
            await Shell.Current.GoToAsync(nameof(SavingsPage));
        });

        public ICommand OpenShoppingListCommand => new Command(async () =>
        {
            await Shell.Current.GoToAsync(nameof(ShoppingListPage));
        });

        public MainMenuPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
    }
}