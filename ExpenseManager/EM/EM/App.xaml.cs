using EM.Data;
using EM.Pages;
using EM.Pages.Autentication;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EM
{
    public partial class App : Application
    {
        private static DatabaseConnection database;
        public static DatabaseConnection Database
        {
            get
            {
                if (database == null)
                {
                    database = new DatabaseConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ExpenseManager_DB.db3"));
                }
                return database;
            }
        }
        public App()
        {
            InitializeComponent();

            NotificationCenter.Current.NotificationTapped += OnLocalNotificationTapped;

            //MainPage = new NavigationPage(new LogInPage());
            //MainPage = new NavigationPage(new AddProductPage());
            //MainPage = new NavigationPage(new MainMenuPage());
            //MainPage = new MainPage();
            //MainPage = new NavigationPage(new ImportReceiptPage());
            MainPage = new AppShell();
        }

        private void OnLocalNotificationTapped(NotificationEventArgs e)
        {
            Current.MainPage.Navigation.PushAsync(new CategoryListPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
