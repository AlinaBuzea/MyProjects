using EM.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EM
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public ICommand BackCommand = new Command(async () =>
        {
            await Shell.Current.GoToAsync("..");
        });
        public AppShell()
        {
            InitializeComponent();
            SetTabBarIsVisible(this, false);
            BindingContext = this;
            Routing.RegisterRoute($"{nameof(CategoryListPage)}", typeof(CategoryListPage));
            Routing.RegisterRoute($"{nameof(AddDataPage)}", typeof(AddDataPage));
            Routing.RegisterRoute($"{nameof(ShoppingListPage)}", typeof(ShoppingListPage));
            Routing.RegisterRoute($"{nameof(BudgetMenuPage)}", typeof(BudgetMenuPage));
            Routing.RegisterRoute($"{nameof(SavingsPage)}", typeof(SavingsPage));
        }

        private void OnMenuItemClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new LogInPage());
        }
    }
}