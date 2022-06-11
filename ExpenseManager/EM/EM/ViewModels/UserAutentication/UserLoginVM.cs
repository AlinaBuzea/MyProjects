using EM.Data;
using EM.Models.Entity;
using EM.Pages;
using EM.Pages.Autentication;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EM.ViewModels.UserAutentication
{
    public class UserLoginVM : BaseViewModel
    {
        #region Fields
        private string name;
        private string pin;
        public string userId { get; set; }
        public UserDB userDB;
        #endregion

        #region Commands
        public ICommand LogInCommand { get; }
        public ICommand SignInCommand { get; }
        public ICommand ForgotPasswordCommand { get; }
        #endregion

        public UserLoginVM()
        {
            LogInCommand = new Command(OnLogInCommand);
            SignInCommand = new Command(OnSignInCommand);
            ForgotPasswordCommand = new Command(OnForgotPasswordCommand);
            userDB = new UserDB();
        }

        #region Properties
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);

        }

        public string Pin
        {
            get => pin;
            set => SetProperty(ref pin, value);

        }

        public string UserId
        {
            get
            {
                return userId;
            }
            set
            {
                userId = value;
                LoadUserId(value);
            }
        }
        #endregion

        public void OnLogInCommand()
        {
            //if (string.IsNullOrWhiteSpace(Name) ||
            //    string.IsNullOrWhiteSpace(Pin))
            //{
            //    await Application.Current.MainPage.DisplayAlert("Alerta", "Numele si Parola sunt obligatorii", "OK");
            //    return;
            //}

            //User currentUser = await App.Database.databaseConn.Table<User>().Where(user => user.UserName.Equals(Name) && user.UserPIN.Equals(Pin)).FirstOrDefaultAsync();
            //List<User> users = await userDB.GetListAsync();
            //foreach(User myUser in users)
            //{
            //    Console.WriteLine("userName: " + myUser.UserName + " userPass:" + myUser.UserPIN);
            //}

            //if (currentUser == null)
            //{
            //    await Application.Current.MainPage.DisplayAlert("Alerta", "Date invalide", "OK");
            //    return;
            //}

            //UserId = currentUser.UserId.ToString();

            UserId = "1";
            Device.BeginInvokeOnMainThread(async()=> await Application.Current.MainPage.Navigation.PushAsync(new MainMenuPage()));

        }

        public void LoadUserId(string userId)
        {
            Task.Run(async () =>
           {
               try
               {
                   var user = await userDB.GetAsync(Int32.Parse(userId));
                   Name = user.UserName;
                   Pin = user.UserPIN;
               }
               catch (Exception)
               {
                   Debug.WriteLine("Failed to Load User");
               }
           });
        }

        private void OnSignInCommand()
        {
            Device.BeginInvokeOnMainThread(async () => await Application.Current.MainPage.Navigation.PushAsync(new RegistrationPage()));
        }

        private void OnForgotPasswordCommand()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                Device.BeginInvokeOnMainThread(() => 
                    Application.Current.MainPage.DisplayAlert("Alert", "Completati numele!", "OK"));
                return;
            }

            User currentUser = Task.Run(async () => await App.Database.databaseConn.Table<User>()
                                                                                    .Where(user => user.UserName.Equals(Name))
                                                                                    .FirstOrDefaultAsync()
                                        ).Result;
            if (currentUser == null)
            {
                Device.BeginInvokeOnMainThread(() => 
                    Application.Current.MainPage.DisplayAlert("Alert", "Nume inexistent!", "OK"));
                return;
            }

            UserId = currentUser.UserId.ToString();
            Pin = "";
            Device.BeginInvokeOnMainThread(async () => await Application.Current.MainPage.Navigation.PushAsync(new ChangePasswordPage()));
        }

    }
}