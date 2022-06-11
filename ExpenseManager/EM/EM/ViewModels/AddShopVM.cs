using EM.Data;
using EM.Models.Entity;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EM.ViewModels
{
    public class AddShopVM : BaseViewModel
    {
        #region Fields
        private string shopId;
        private string shopName;
        private string shopAddress;

        ShopDB shopDB;
        List<Shop> _list;
        #endregion

        #region Commands
        public ICommand CancelCommand { get; }
        public ICommand SaveCommand { get; }
        #endregion

        public AddShopVM()
        {
            shopDB = new ShopDB();
            CancelCommand = new Command(OnCancelCommand);
            SaveCommand = new Command(OnSaveCommand);
            _list = new List<Shop>();
        }

        #region Properties
        public string ShopId
        {
            get => shopId;
            set => SetProperty(ref shopId, value);
        }
        public string ShopName
        {
            get => shopName;
            set => SetProperty(ref shopName, value);
        }
        public string ShopAddress
        {
            get => shopAddress;
            set => SetProperty(ref shopAddress, value);
        }
        #endregion

        public void OnCancelCommand()
        {
            Device.BeginInvokeOnMainThread(async () =>
               await App.Current.MainPage.Navigation.PopAsync()
                );
        }

        public void OnSaveCommand()
        {
            if (string.IsNullOrWhiteSpace(ShopName))
            {
                Device.BeginInvokeOnMainThread(() =>
                        Application.Current.MainPage.DisplayAlert("Alerta", "Introduceti denumirea magazinului!", "OK"));
                return;
            }
            Task.Run(async () => _list = await shopDB.GetListAsync());

            if (_list.Find(shop => shop.ShopName.Equals(ShopName) && shop.ShopAddress.Equals(ShopAddress)) != null)
            {
                Device.BeginInvokeOnMainThread(() =>
                        Application.Current.MainPage.DisplayAlert("Alerta", "Magazinul introdus exista deja in lista!", "OK"));
                return;
            }

            Shop newShop = new Shop();
            newShop.ShopAddress = ShopAddress;
            newShop.ShopName = ShopName;

            Task.Run(async () =>
            {
                await shopDB.SaveAsync(newShop);

                _list = await shopDB.GetListAsync();
            });
            foreach (Shop shop in _list)
            {
                Console.WriteLine("shopId = " + shop.Id + " shopName = " + shop.ShopName + " shopAddress = " + shop.ShopAddress);
            }
            ReinitializeFields();

            //await App.Current.MainPage.Navigation.PopAsync();
        }

        private void ReinitializeFields()
        {
            ShopAddress = "";
            ShopName = "";
        }
    }
}
