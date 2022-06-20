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
    public class UserRegistrationVM : BaseViewModel
    {
        #region Fields
        private string userId;
        private string email;
        private string pin;
        private string confirmPin;
        private bool isValidEmail;
        private UserDB userDB;
        private List<User> _list;
        #endregion

        public ICommand SaveCommand { get; }

        public UserRegistrationVM()
        {
            SaveCommand = new Command(OnSaveCommand);
            userDB = new UserDB();
        }

        #region Properties
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);

        }

        public string Pin
        {
            get => pin;
            set => SetProperty(ref pin, value);

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
        public bool IsValidEmail
        {
            get => isValidEmail;
            set => SetProperty(ref isValidEmail, value);
        }
        #endregion
        public async void OnSaveCommand()
        {
            if (string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Pin) ||
                string.IsNullOrWhiteSpace(ConfirmPin))
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Toate campurile sunt obligatorii!", "OK");
                return;
            }

            if (!IsValidEmail)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Sirul de caractere inscris in campul \"Email\" NU este o adresa de email", "OK");
                return;
            }

            if (!ConfirmPin.Equals(Pin))
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Valorile campurilor Parola si Confirma Parola trebuie sa coincida", "OK");
                return;
            }

            User newUser = new User();
            newUser.UserEmail = Email;
            newUser.UserPIN = Pin;

            Console.WriteLine(" UserEmail: " + newUser.UserEmail + " UserPIN: " + newUser.UserPIN);
            _list = await userDB.GetListAsync();

            if (_list.Count != 0)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Aveti deja datele de securitate setate", "OK");
                return;
            }

            await userDB.SaveAsync(newUser);
            _list = await userDB.GetListAsync();
            foreach (User user in _list)
            {
                Console.WriteLine("UserID: " + user.UserId + " UserEmail: " + user.UserEmail + " UserPIN: " + user.UserPIN);
            }

            await Application.Current.MainPage.Navigation.PopAsync();
        }

        public async void LoadUserId(string userId)
        {
            try
            {
                var user = await userDB.GetAsync(Int32.Parse(userId));
                UserId = user.UserId.ToString();
                Email = user.UserEmail;
                Pin = user.UserPIN;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load User");
            }
        }
    }
}