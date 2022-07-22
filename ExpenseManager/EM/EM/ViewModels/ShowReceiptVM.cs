using EM.Pages;
using EM.Processing;
using MvvmHelpers;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EM.ViewModels
{
    public class ShowReceiptVM : BaseViewModel
    {
        #region Fields
        private bool isRunning;
        IEnumerable<FileResult> imageResults;
        private List<Xamarin.Forms.ImageSource> photoCollection;
        #endregion

        public ICommand ProccessReceiptCommand { get; }

        public ShowReceiptVM(IEnumerable<FileResult> result)
        {
            imageResults = new List<FileResult>(result);
            InitializeCollectionItemsSource();
            ProccessReceiptCommand = new Command(OnProccessReceiptCommand);
            IsRunning = false;
        }

        public ShowReceiptVM(FileResult result)
        {
            imageResults = new List<FileResult>() { result };
            InitializeCollectionItemsSource();
            ProccessReceiptCommand = new Command(OnProccessReceiptCommand);
            IsRunning = false;
        }

        #region Properties
        public bool IsRunning
        {
            get => isRunning;
            set => SetProperty(ref isRunning, value);
        }
        public List<Xamarin.Forms.ImageSource> PhotoCollection
        {
            get => photoCollection;
            set => SetProperty(ref photoCollection, value);
        }
        #endregion

        private async void OnProccessReceiptCommand()
        {
            IsRunning = true;
            List<string> receiptsText = new List<string>();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                try
                {
                    foreach (FileResult result1 in imageResults)
                    {
                        var stream = await result1.OpenReadAsync();
                        await stream.CopyToAsync(memoryStream);
                        receiptsText.Add(await ConvertByteArrayImageTextToTextFileText(memoryStream.ToArray()));
                        foreach (string receiptTextString in receiptsText)
                        {
                            InformationExtractor informationExtractor = new InformationExtractor();
                            var filteredInformationTuples = informationExtractor.GetFilteredInformation(receiptTextString);
                            var currentPage = App.Current.MainPage.Navigation.NavigationStack[App.Current.MainPage.Navigation.NavigationStack.Count - 1];
                            IsRunning = false;
                            if (currentPage==null || !currentPage.GetType().Name.Equals(nameof(ShowReceiptPage)))
                            {
                                return;
                            }
                            await App.Current.MainPage.Navigation.PushAsync(new ReceiptInformationPage(filteredInformationTuples));
                        }
                    }
                }
                catch (Exception)
                {
                    int nbOfPagesInTheNavigationStack = App.Current.MainPage.Navigation.NavigationStack.Count;
                    var currentPage = nbOfPagesInTheNavigationStack > 1 ? App.Current.MainPage.Navigation.NavigationStack[nbOfPagesInTheNavigationStack - 1] : null;
                    if (currentPage == null || !currentPage.GetType().Name.Equals(nameof(ShowReceiptPage)))
                    {
                        IsRunning = false;
                        return;
                    }
                }
            }
        }


        private async void InitializeCollectionItemsSource()
        {
            List<Xamarin.Forms.ImageSource> imageSources = new List<Xamarin.Forms.ImageSource>();
            foreach (FileResult result1 in imageResults)
            {
                var stream = await result1.OpenReadAsync();
                imageSources.Add(Xamarin.Forms.ImageSource.FromStream(() => stream));
            }
            PhotoCollection = new List<Xamarin.Forms.ImageSource>(imageSources);
        }

        public async Task<string> ConvertByteArrayImageTextToTextFileText(byte[] imageByteArray)
        {
            var client = new RestClient("http://" + App.pythonServerHost+ ":" + App.pythonPort);

            Dictionary<string, byte[]> body = new Dictionary<string, byte[]>();
            body.Add("imageBytes", imageByteArray);

            var jsonBody = JsonConvert.SerializeObject(body);

            var request = new RestRequest().AddJsonBody(jsonBody);

            var responseMessage = await client.PostAsync(request);
            if (responseMessage != null)
            {
                return responseMessage.Content.ToString();
            }
            return null;
        }
    }
}