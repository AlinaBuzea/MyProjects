using System.Windows;

namespace CheckersGame
{
    public partial class DameGameOver : Window
    {
        public DameGameOver()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void btnStatistics_Click(object sender, RoutedEventArgs e)
        {
            GameOverWinnersVM gameOverWinnersVM = this.DataContext as GameOverWinnersVM;
            gameOverWinnersVM.winnerCol = lblWinnerColor.Content.ToString();
            gameOverWinnersVM.UpdateEvidence();
            lbStatistics.Visibility = Visibility.Visible;
        }
    }
}
