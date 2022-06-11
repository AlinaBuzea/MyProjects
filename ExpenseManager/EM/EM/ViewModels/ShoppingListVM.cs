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
    public class ShoppingListVM : BaseViewModel
    {
        #region Fields
        private int id;
        private string item;
        private bool isAPriority;
        private bool isBought;

        private ShoppingItemDB shoppingItemDB;
        List<ShoppingItem> shoppingListItems;
        List<ShoppingItem> deletedListItems;
        #endregion

        #region Commands
        public ICommand AddNewRowCommand { get; set; }
        public ICommand SaveListCommand { get; set; }
        public ICommand DeleteListCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        #endregion


        public ShoppingListVM()
        {
            shoppingItemDB = new ShoppingItemDB();
            AddNewRowCommand = new Command(OnAddNewRowCommand);
            CancelCommand = new Command(OnCancelCommand);
            SaveListCommand = new Command(OnSaveListCommand);
            DeleteListCommand = new Command(OnDeleteListCommand);
            InitializeListByPriorityThenByIsBought();
            deletedListItems = new List<ShoppingItem>();
        }

        #region Properties
        public int Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        public string Item
        {
            get => item;
            set => SetProperty(ref item, value);
        }

        public bool IsAPriority
        {
            get => isAPriority;
            set => SetProperty(ref isAPriority, value);
        }

        public bool IsBought
        {
            get => isBought;
            set => SetProperty(ref isBought, value);
        }

        public List<ShoppingItem> ShoppingListItems
        {
            get => shoppingListItems;
            set => SetProperty(ref shoppingListItems, value);
        }
        #endregion

        private void InitializeListByPriorityThenByIsBought()
        {
            ShoppingListItems = Task.Run(async () => await shoppingItemDB.GetListOrderByIsNotBoughtThenByIsAPriorityAsync()).Result;
            Task.Run(() =>
            {
                foreach (ShoppingItem shoppingItem in ShoppingListItems)
                {
                    Console.WriteLine("item: " + shoppingItem.Item + "  isAPriority: " + shoppingItem.IsAPriority +
                        "  isBought: " + shoppingItem.IsBought);
                }
            });
        }

        private void InitializeList()
        {
            ShoppingListItems = Task.Run(async () => await shoppingItemDB.GetListAsync()).Result;
            Task.Run(() =>
            {
                foreach (ShoppingItem shoppingItem in ShoppingListItems)
                {
                    Console.WriteLine("item: " + shoppingItem.Item + "  isAPriority: " + shoppingItem.IsAPriority +
                        "  isBought: " + shoppingItem.IsBought);
                }
            });
        }

        private void OnAddNewRowCommand()
        {
            List<ShoppingItem> list = new List<ShoppingItem>(ShoppingListItems);
            list.Add(new ShoppingItem() { IsAPriority = false, IsBought = false });
            ShoppingListItems = new List<ShoppingItem>(list);
        }

        private void OnDeleteListCommand()
        {
            ShoppingListItems = Task.Run(() =>
            {
                foreach (ShoppingItem shoppingItem in ShoppingListItems)
                {
                    Console.WriteLine("deleted Item: " + shoppingItem.Item + "  isAPriority: " + shoppingItem.IsAPriority +
                        "  isBought: " + shoppingItem.IsBought);
                    deletedListItems.Add(shoppingItem);
                }
                return new List<ShoppingItem>();
            }).Result;

        }

        private void OnSaveListCommand()
        {
            Task.Run(async () =>
            {
                foreach (ShoppingItem deletedItem in deletedListItems)
                {
                    Console.WriteLine("item: " + deletedItem.Item + "  isAPriority: " + deletedItem.IsAPriority +
                        "  isBought: " + deletedItem.IsBought);
                    if (deletedItem.Item != null)
                    {
                        if (deletedItem.Id != 0)
                        {
                            await shoppingItemDB.DeleteAsync(deletedItem);
                        }
                    }
                }
                deletedListItems = new List<ShoppingItem>();
            }).Wait();
            Task.Run(async () =>
            {
                foreach (ShoppingItem shoppingItem in ShoppingListItems)
                {
                    Console.WriteLine("item: " + shoppingItem.Item + "  isAPriority: " + shoppingItem.IsAPriority +
                        "  isBought: " + shoppingItem.IsBought);
                    if (shoppingItem.Item != null)
                    {
                        await shoppingItemDB.SaveAsync(shoppingItem);
                    }
                }
            }).Wait();
            ShoppingListItems = Task.Run(async () => { return await shoppingItemDB.GetListOrderByIsNotBoughtThenByIsAPriorityAsync(); }).Result;
        }

        private async void OnCancelCommand()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
