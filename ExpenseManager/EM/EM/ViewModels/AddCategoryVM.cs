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

namespace EM.ViewModels
{
    [QueryProperty(nameof(CategoryId), nameof(CategoryId))]
    public class AddCategoryVM : BaseViewModel
    {
        #region Fields
        public string Id { get; set; }
        private string categoryName;
        private string orderIndex;
        private string categoryId;
        private CategoryDB categoryDB;
        List<Category> _list;
        #endregion

        #region Commands
        public ICommand CancelCommand { get; }
        public ICommand SaveCommand { get; }
        #endregion

        public AddCategoryVM()
        {
            CancelCommand = new Command(OnCancelCommand);
            SaveCommand = new Command(OnSaveCommand);
            categoryDB = new CategoryDB();

        }

        #region Properties
        public string CategoryName
        {
            get => categoryName;
            set => SetProperty(ref categoryName, value);
        }

        public string OrderIndex
        {
            get => orderIndex;
            set => SetProperty(ref orderIndex, value);
        }

        public string CategoryId
        {
            get => categoryId;
            set
            {
                categoryId = value;
                LoadProdId(value);
            }
        }

        #endregion

        private void LoadProdId(string categoryId)
        {
            Task.Run(async () =>
            {
                try
                {
                    var category = await categoryDB.GetAsync(Int32.Parse(categoryId));
                    CategoryName = category.CategoryName;
                    OrderIndex = category.OrderIndex.ToString();

                }
                catch (Exception)
                {
                    Debug.WriteLine("CategoryId couldn't load!");
                }
            });
        }

        private void OnCancelCommand()
        {
            Device.BeginInvokeOnMainThread(async () => await Application.Current.MainPage.Navigation.PopAsync());
        }

        private void OnSaveCommand()
        {
            if (string.IsNullOrWhiteSpace(CategoryName))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.DisplayAlert("Alerta", "Introduceti Categoria!", "OK");
                });
                return;
            }

            Task.Run(async () =>
            {
                _list = await categoryDB.GetListAsync();
                if (_list.Find(category => category.CategoryName.Equals(CategoryName)) == null)
                {
                    Category newCategory = new Category();
                    newCategory.CategoryName = CategoryName;
                    newCategory.OrderIndex = -1;

                    await categoryDB.SaveAsync(newCategory);

                    _list = await categoryDB.GetListAsync();
                    foreach (Category category in _list)
                    {
                        Console.WriteLine("CategoryID: " + category.CategoryId + " CategoryName: " + category.CategoryName +
                            " orderIndex: " + category.OrderIndex);
                    }
                }
            });

            Device.BeginInvokeOnMainThread(async () => await Application.Current.MainPage.Navigation.PopAsync());
        }
    }
}
