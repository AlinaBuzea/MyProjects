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
        private string name;
        private string email;
        private string pin;
        private string confirmPin;
        public string Id { get; set; }
        public UserDB userDB;
        private List<User> _list;
        #endregion

        public ICommand SaveCommand { get; }

        public UserRegistrationVM()
        {
            SaveCommand = new Command(OnSaveCommand);
            userDB = new UserDB();
        }

        #region Properties
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);

        }

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
        #endregion
        public void OnSaveCommand()
        {
            if (string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Pin) ||
                string.IsNullOrWhiteSpace(ConfirmPin))
            {
                Device.BeginInvokeOnMainThread(() => 
                    Application.Current.MainPage.DisplayAlert("Alert", "Toate campurile sunt obligatorii!", "OK"));
                return;
            }

            if (ConfirmPin.Equals(Pin))
            {
                User newUser = new User();
                newUser.UserEmail = Email;
                newUser.UserName = Name;
                newUser.UserPIN = Pin;

                Console.WriteLine("UserName: " + newUser.UserName + " UserEmail: " + newUser.UserEmail + " UserPIN: " + newUser.UserPIN);

                User currentUser = Task.Run(async()=> 
                                        await App.Database.databaseConn.Table<User>()
                                                                        .Where(user => user.UserName.Equals(Name) || user.UserEmail.Equals(Email))
                                                                        .FirstOrDefaultAsync()
                                        ).Result;

                if (currentUser != null)
                {
                    if (currentUser.UserName.Equals(newUser.UserName))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                            Application.Current.MainPage.DisplayAlert("Alerta", "Numele exista deja in baza de date! Alege altul!", "OK"));
                        return;
                    }
                    if (currentUser.UserEmail.Equals(newUser.UserEmail))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                            Application.Current.MainPage.DisplayAlert("Alerta", "Aceasta adresa de email exista deja in baza de date! Alege alta!", "OK"));
                        return;
                    }
                }

                Task.Run(async () =>
                {
                    await userDB.SaveAsync(newUser);
                    _list = await userDB.GetListAsync();
                
                    foreach (User user in _list)
                    {
                        Console.WriteLine("UserID: " + user.UserId + " UserName: " + user.UserName + " UserEmail: " + user.UserEmail + " UserPIN: " + user.UserPIN);
                    }
                });
                Device.BeginInvokeOnMainThread(async () => await Application.Current.MainPage.Navigation.PopAsync());
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                    Application.Current.MainPage.DisplayAlert("Alert", "Valorile campurilor Parola si Confirma Parola trebuie sa coincida", "OK"));
            }
        }

        public void LoadUserId(string userId)
        {
            try
            {
                var user = Task.Run(async () => await userDB.GetAsync(Int32.Parse(userId))).Result;
                Id = user.UserId.ToString();
                Name = user.UserName;
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
