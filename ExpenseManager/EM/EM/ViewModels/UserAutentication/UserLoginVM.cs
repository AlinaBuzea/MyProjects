using EM.Data;
using EM.Models.Entity;
using EM.Pages;
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
        private string pin;
        public string userId;
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
        public string Pin
        {
            get => pin;
            set => SetProperty(ref pin, value);

        }

        public string UserId
        {
            get => userId;
            set => SetProperty(ref userId, value);
        }
        #endregion

        public async void OnLogInCommand()
        {
            User currentUser = await userDB.GetAsync(1);
            if (currentUser == null)
            {
                await Application.Current.MainPage.DisplayAlert("Nu ai stabilit nici-o parola", "Setaza-ti o parola!", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(Pin))
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Introduce parola!", "OK");
                return;
            }

            List<User> users = await userDB.GetListAsync();
            foreach (User myUser in users)
            {
                Console.WriteLine("userEmail: " + myUser.UserEmail + " userPass:" + myUser.UserPIN);
            }

            if (!currentUser.UserPIN.Equals(Pin))
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Parola incorecta", "OK");
                return;
            }

            Application.Current.MainPage = new AppShell();
        }

        private async void OnSignInCommand()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RegistrationPage());
        }

        private async void OnForgotPasswordCommand()
        {
            User currentUser = await userDB.GetAsync(1);
            if (currentUser == null)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Nu te-ai inregistrat. Te rugam inregistreaza-te!", "OK");
                return;
            }

            UserId = currentUser.UserId.ToString();
            Pin = "";
            await Application.Current.MainPage.Navigation.PushAsync(new ChangePasswordPage());
        }

    }
}