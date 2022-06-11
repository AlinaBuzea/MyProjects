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
        private string recoveryCode;
        private string email;
        private string newPin;
        private string confirmPin;
        public string Id { get; set; }
        public UserDB userDB;
        private List<User> _list;
        #endregion

        private string emailRecoveryCode;

        #region Commands
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand SendRecoveryCodeCommand { get; }
        #endregion

        public UserChangePassVM()
        {
            SaveCommand = new Command(OnSaveCommand);
            CancelCommand = new Command(OnCancelCommand);
            SendRecoveryCodeCommand = new Command(OnSendRecoveryCodeCommand);
            userDB = new UserDB();
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
        #endregion

        public void OnSaveCommand()
        {
            if (string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(NewPin) ||
                string.IsNullOrWhiteSpace(ConfirmPin))
            {
                Device.BeginInvokeOnMainThread(() => 
                    Application.Current.MainPage.DisplayAlert("Alerta", "Toate campurile sunt obligatorii!", "OK")
                    );
                return;
            }

            if (ConfirmPin.Equals(NewPin))
            {
                Console.WriteLine(" UserEmail: " + Email + " UserPIN: " + NewPin);

                User currentUser = Task.Run(async () => 
                                        await App.Database.databaseConn.Table<User>().Where(user => user.UserEmail.Equals(Email)).FirstOrDefaultAsync()
                                        ).Result;

                if (currentUser == null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                        Application.Current.MainPage.DisplayAlert("Alerta", "Adresa de email nu a fost gasita!", "OK")
                        );
                    return;
                }

                currentUser.UserPIN = NewPin;
                Task.Run(async () =>
                {
                    await userDB.SaveAsync(currentUser);
                    _list = await userDB.GetListAsync();
                });
                foreach (User user in _list)
                {
                    Console.WriteLine("UserID: " + user.UserId + " UserName: " + user.UserName + " UserEmail: " + user.UserEmail + " UserPIN: " + user.UserPIN);
                }
                Device.BeginInvokeOnMainThread(async () => await Application.Current.MainPage.Navigation.PopAsync());
            }
        }

        private void OnCancelCommand()
        {
            Device.BeginInvokeOnMainThread(async () => await Application.Current.MainPage.Navigation.PopAsync());
        }

        public void LoadUserId(string userId)
        {
            try
            {
                var user = Task.Run(async () => await userDB.GetAsync(Int32.Parse(userId))).Result;
                Id = user.UserId.ToString();
                Email = user.UserEmail;
                NewPin = user.UserPIN;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load User");
            }
        }

        private void OnSendRecoveryCodeCommand()
        {
            emailRecoveryCode = "asY34p";
        }

        private void ReinitializeFields()
        {
            Email = "";
            NewPin = "";
            ConfirmPin = "";
        }
    }
}
