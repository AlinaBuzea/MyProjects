using EM.Data;
using EM.Models.Entity;
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
    [QueryProperty(nameof(UserId), nameof(UserId))]
    public class UserChangePassVM : BaseViewModel
    {
        #region Fields
        private string userId;
        private string email;
        private string recoveryCode;
        private string newPin;
        private string confirmPin;

        private bool isEmailUIElemEnabled;
        private bool isCodeUIElemEnabled;
        private bool isPasswordUIElemEnabled;
        private string emailImageSource;
        private bool isValidEmail;

        public UserDB userDB;
        private List<User> _list;

        private const string emailImage = "Email.png";
        private const string emailImage_Incorrect = "IncorrectEmailAddress.png";
        private const string emailImage_Correct = "CorrectEmailAddress.png";
        #endregion

        #region Commands
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand VerifyEmailAddressCommand { get; }
        #endregion

        public UserChangePassVM()
        {
            IsEmailUIElemEnabled = true;
            IsPasswordUIElemEnabled = false;
            SaveCommand = new Command(OnSaveCommand);
            CancelCommand = new Command(OnCancelCommand);
            VerifyEmailAddressCommand = new Command(OnVerifyEmailAddressCommand);
            userDB = new UserDB();
            EmailImageSource = emailImage;
        }

        #region Properties
        public string RecoveryCode
        {
            get => recoveryCode;
            set => SetProperty(ref recoveryCode, value);

        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);

        }

        public string NewPin
        {
            get => newPin;
            set => SetProperty(ref newPin, value);

        }
        public string ConfirmPin
        {
            get => confirmPin;
            set => SetProperty(ref confirmPin, value);

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
        public string EmailImageSource
        {
            get => emailImageSource;
            set => SetProperty(ref emailImageSource, value);
        }

        public bool IsValidEmail
        {
            get => isValidEmail;
            set => SetProperty(ref isValidEmail, value);
        }

        public bool IsEmailUIElemEnabled
        {
            get => isEmailUIElemEnabled;
            set => SetProperty(ref isEmailUIElemEnabled, value);
        }
        public bool IsCodeUIElemEnabled
        {
            get => isCodeUIElemEnabled;
            set => SetProperty(ref isCodeUIElemEnabled, value);
        }
        public bool IsPasswordUIElemEnabled
        {
            get => isPasswordUIElemEnabled;
            set => SetProperty(ref isPasswordUIElemEnabled, value);
        }
        #endregion

        public async void OnSaveCommand()
        {
            if (string.IsNullOrWhiteSpace(NewPin) ||
                 string.IsNullOrWhiteSpace(ConfirmPin))
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Toate campurile sunt obligatorii!", "OK");
                return;
            }

            if (!ConfirmPin.Equals(NewPin))
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Valorile campurilor Parola Noua si Confirma Parola trebuie sa coincida", "OK");
                return;
            }

            User currentUser = await userDB.GetByEmailAsync(Email);

            if (currentUser == null)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Adresa de email nu a fost gasita!", "OK");
                return;
            }

            currentUser.UserPIN = NewPin;
            await userDB.SaveAsync(currentUser);
            _list = await userDB.GetListAsync();
            foreach (User user in _list)
            {
                Console.WriteLine("UserID: " + user.UserId + " UserEmail: " + user.UserEmail + " UserPIN: " + user.UserPIN);
            }

            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void OnCancelCommand()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        public async void LoadUserId(string userId)
        {
            try
            {
                var user = await userDB.GetAsync(Int32.Parse(userId));
                UserId = user.UserId.ToString();
                Email = user.UserEmail;
                NewPin = user.UserPIN;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load User");
            }
        }

        private async void OnVerifyEmailAddressCommand()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                EmailImageSource = emailImage_Incorrect;
                await Application.Current.MainPage.DisplayAlert("Alerta", "Introduceti adresa de email!", "OK");
                return;
            }

            if (!IsValidEmail)
            {
                EmailImageSource = emailImage_Incorrect;
                await Application.Current.MainPage.DisplayAlert("Alerta", "Sirul de caractere inscris in campul \"Email\" NU este o adresa de email", "OK");
                return;
            }

            User currentUser = await userDB.GetByEmailAsync(Email);
            if (currentUser == null)
            {
                EmailImageSource = emailImage_Incorrect;
                await Application.Current.MainPage.DisplayAlert("Alerta", "Adresa de email nu a fost gasita!", "OK");
                return;
            }

            IsEmailUIElemEnabled = false;
            IsPasswordUIElemEnabled = true;
            EmailImageSource = emailImage_Correct;
        }

        private void ReinitializeFields()
        {
            Email = "";
            NewPin = "";
            ConfirmPin = "";
        }
    }
}
