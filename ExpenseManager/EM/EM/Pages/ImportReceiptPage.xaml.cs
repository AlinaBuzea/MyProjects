using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EM.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImportReceiptPage : ContentPage
    {
        public ICommand AccessCameraCommand => new Command(async () =>
        {
            var status = await Permissions.RequestAsync<Permissions.Camera>();
            if (status.Equals(PermissionStatus.Granted))
            {
                FileResult result = await MediaPicker.CapturePhotoAsync();
                ShowImage(result);
            }

        });

        public ICommand ImportImageFromGaleryCommand => new Command(async () =>
        {
            var status = await Permissions.RequestAsync<Permissions.StorageRead>();
            if (status.Equals(PermissionStatus.Granted))
            {
                FileResult result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Selecteaza o poza"
                });

                ShowImage(result);
            }
        });

        private async void ShowImage(FileResult result)
        {
            if (result != null)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ShowReceiptPage(result));
            }
        }

        public ICommand ImportImagesCommand => new Command(async () =>
        {
            IEnumerable<FileResult> result = await FilePicker.PickMultipleAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Selecteaza una sau mai multe imagini"
            });

            if (result != null)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ShowReceiptPage(result));
            }

        });

        public ICommand ImportPDFCommand => new Command(async () =>
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Pdf,
                PickerTitle = "Selecteaza PDF"
            });

            ///de completat cu PDF -> images
        });

        public ImportReceiptPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
    }
}