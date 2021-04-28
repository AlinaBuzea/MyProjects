using System.Windows;


namespace CheckersGame
{
    public partial class IntermediateMenu : Window
    {
        public IntermediateMenu()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if(cbChooseSavedGame.IsChecked==true)
            {
                GameInitializationVM.ChooseSavedGame = "CurrentState.txt";
            }
            else
            {
                GameInitializationVM.ChooseSavedGame = "Initial_state.txt";
            }
            GameView gmv = new GameView();
            gmv.lblPlayer1Name.Content = tbNumeJucatorPieseAlbe.Text;
            gmv.lblPlayer2Name.Content = tbNumeJucatorPieseRosii.Text;
            gmv.lbl1SarituriMultiple.Content = cbMultipleJumps.IsChecked;
            gmv.lbl2SarituriMultiple.Content = cbMultipleJumps.IsChecked;
            gmv.Show();
            this.Close();
        }
    }
}


