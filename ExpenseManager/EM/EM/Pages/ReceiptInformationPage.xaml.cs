using EM.Processing;
using EM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EM.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceiptInformationPage : ContentPage
    {
        public ReceiptInformationPage(List<Tuple<InformationExtractor.InformationType, string>> filteredInformationTuples)
        {
            InitializeComponent();
            BindingContext = new ReceiptInformationVM(filteredInformationTuples);
        }
    }
}