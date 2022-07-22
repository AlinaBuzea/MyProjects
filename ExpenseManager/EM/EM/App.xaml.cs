using EM.Data;
using EM.Pages;
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
        internal const string pythonServerHost = "192.168.43.186";/// se va inlocui cu adresa locala de internet la care sunt conectate 
                                                                  ///laptopul/desktop-ul si device-ul mobil pe care este rulat programul
        internal const int pythonPort = 9000;

        private static DatabaseConnection database;
        public static DatabaseConnection Database
        {
            get
            {
                if (database == null)
                {
                    database = new DatabaseConnection(Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ExpenseManager_DB.db3"));
                }
                return database;
            }
        }
        public App()
        {
            InitializeComponent();

            NotificationCenter.Current.NotificationTapped += OnLocalNotificationTapped;

            MainPage = new NavigationPage(new LogInPage());

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
