using System.Windows.Controls;

namespace CheckersGame
{
    public partial class AboutView : UserControl
    {
        public AboutView()
        {
            InitializeComponent();
            AboutClass about = new AboutClass();
        }
    }
}
