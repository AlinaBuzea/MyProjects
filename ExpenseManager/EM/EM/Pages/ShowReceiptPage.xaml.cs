using EM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EM.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowReceiptPage : ContentPage
    {
        public ShowReceiptPage(IEnumerable<FileResult> result)
        {
            InitializeComponent();
            BindingContext = new ShowReceiptVM(result);
        }

        public ShowReceiptPage(FileResult result)
        {
            InitializeComponent();
            BindingContext = new ShowReceiptVM(result);
        }
    }
}