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
        private readonly string pythonServerHost = "192.168.100.14"; //facultate: "10.146.1.102"
        private readonly int pythonPort = 9000;

        IEnumerable<FileResult> imageResults;
        private List<Xamarin.Forms.ImageSource> photoCollection;

        public ICommand ProccessReceiptCommand { get; }

        public ShowReceiptVM(IEnumerable<FileResult> result)
        {
            imageResults = new List<FileResult>(result);
            InitializeCollectionItemsSource();
            ProccessReceiptCommand = new Command(OnProccessReceiptCommand);
        }

        public ShowReceiptVM(FileResult result)
        {
            imageResults = new List<FileResult>() { result };
            InitializeCollectionItemsSource();
        }

        public List<Xamarin.Forms.ImageSource> PhotoCollection
        {
            get => photoCollection;
            set => SetProperty(ref photoCollection, value);
        }

        private async void OnProccessReceiptCommand()//nu functioneaza pt mai multe poze
        {
            List<string> receiptsText = new List<string>();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                foreach (FileResult result1 in imageResults)
                {
                    var stream = await result1.OpenReadAsync();
                    await stream.CopyToAsync(memoryStream);
                    receiptsText.Add(await ConvertByteArrayImageTextToTextFileText(memoryStream.ToArray()));

                    foreach (string s in receiptsText)
                        Console.WriteLine("s:" + s);
                    foreach (string receiptTextString in receiptsText)
                    {
                        InformationExtractor informationExtractor = new InformationExtractor();
                        var filteredInformationTuples = informationExtractor.GetFilteredInformation(receiptTextString);
                        await App.Current.MainPage.Navigation.PushAsync(new ReceiptInformationPage(filteredInformationTuples));
                    }
                    //interpreteaza textul 

                    //introdu elementele intr-o lista de obiecte Product

                    //transmite-le catre o pagina cu lista de Produse(la fiecare cate o optiune de a modifica datele produsului)
                    // + un buton la sfarsit de "adauga produsele" (doar la apasarea lui, produsele vor fi introduse in BD) - nevoie de ViewModel
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
            var client = new RestClient("http://" + pythonServerHost + ":" + pythonPort);

            Dictionary<string, byte[]> body = new Dictionary<string, byte[]>();
            body.Add("imageBytes", imageByteArray);

            var jsonBody = JsonConvert.SerializeObject(body);

            var request = new RestRequest().AddJsonBody(jsonBody);

            var responseMessage = await client.PostAsync(request);

            var x = responseMessage.Content.ToString();
            return x;
        }
    }
}
