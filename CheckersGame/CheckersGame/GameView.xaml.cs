using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace CheckersGame
{
    public partial class GameView : Window
    {
        public GameView(string boardState="InitialState.txt")
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void piesa_Click(object sender, RoutedEventArgs e)
        {
            FieldVM myFieldVM = this.DataContext as FieldVM;
            myFieldVM.MultipleJumps = InitializeMultipleJumps();
            if (myFieldVM.CurrentField == null && (((sender as System.Windows.Controls.Button).Tag) as Field).FieldPiece != null)
            {
                if ((((sender as System.Windows.Controls.Button).Tag) as Field).FieldPiece.Color == myFieldVM.CurrentTurn)
                {
                    myFieldVM.CurrentField = ((sender as System.Windows.Controls.Button).Tag) as Field;
                }
            }
            else if (myFieldVM.CurrentField != null)
            {
                if (!myFieldVM.NoMoreMovesLeft(myFieldVM.CurrentField.FieldPiece.Color))
                {
                    VerifyMoves(myFieldVM);
                }
                else
                {
                    if ((((sender as System.Windows.Controls.Button).Tag) as Field).FieldPiece != null && myFieldVM.CurrentField.FieldPiece != null)
                    {
                        if ((((sender as System.Windows.Controls.Button).Tag) as Field).FieldPiece.Color == myFieldVM.CurrentField.FieldPiece.Color)
                        {
                            myFieldVM.CurrentField = ((sender as System.Windows.Controls.Button).Tag) as Field;
                        }
                    }
                    if ((((sender as System.Windows.Controls.Button).Tag) as Field) != myFieldVM.CurrentField)
                    {
                        Piece.PieceCol currentColor = myFieldVM.CurrentField.FieldPiece.Color;
                        myFieldVM.NewPositionField = ((sender as System.Windows.Controls.Button).Tag) as Field;

                        if (myFieldVM.ChangePiecePosition(myFieldVM.NewPositionField.fieldCoordinates.Item1, myFieldVM.NewPositionField.fieldCoordinates.Item2))
                        {
                            if (myFieldVM.jumpOverPiece && myFieldVM.MultipleJumps)
                            {
                                myFieldVM.CurrentField = myFieldVM.NewPositionField;
                                int step;
                                if (myFieldVM.CurrentField.FieldPiece.Color == Piece.PieceCol.RED)
                                {
                                    step = 1;
                                }
                                else
                                {
                                    step = -1;
                                }
                                if (!(myFieldVM.ValidateMove2Steps(step, myFieldVM.CurrentField.fieldCoordinates.Item1 - 2 * step, myFieldVM.CurrentField.fieldCoordinates.Item2 - 2) ||
                                 myFieldVM.ValidateMove2Steps(step, myFieldVM.CurrentField.fieldCoordinates.Item1 - 2 * step, myFieldVM.CurrentField.fieldCoordinates.Item2 + 2) ||
                                 (myFieldVM.CurrentField.FieldPiece.IsKing && (myFieldVM.ValidateMove2Steps(step, myFieldVM.CurrentField.fieldCoordinates.Item1 + 2 * step, myFieldVM.CurrentField.fieldCoordinates.Item2 - 2) ||
                                 myFieldVM.ValidateMove2Steps(step, myFieldVM.CurrentField.fieldCoordinates.Item1 + 2 * step, myFieldVM.CurrentField.fieldCoordinates.Item2 + 2)))))
                                {
                                    myFieldVM.CurrentField = null;
                                    myFieldVM.NewPositionField = null;
                                    myFieldVM.CurrentTurn = 1 - currentColor;
                                    ChangeTurn(myFieldVM.CurrentTurn);
                                    myFieldVM.jumpOverPiece = false;
                                }
                            }
                            else
                            {
                                myFieldVM.CurrentTurn = 1 - currentColor;
                                ChangeTurn(myFieldVM.CurrentTurn);
                                myFieldVM.CurrentField = null;
                            }
                            if (myFieldVM.RedPiecesNumberInGame == 0)
                            {
                                EndGame(Piece.PieceCol.WHITE, lblPlayer1Name.Content.ToString());
                            }
                            else if (myFieldVM.WhitePiecesNumberInGame == 0)
                            {
                                EndGame(Piece.PieceCol.RED, lblPlayer2Name.Content.ToString());
                            }
                        }
                    }

                }

            }
        }
        private void ChangeTurn(Piece.PieceCol col)
        {
            if (col == Piece.PieceCol.WHITE)
            {
                Red_Turn.Visibility = Visibility.Hidden;
                lblRed_Turn.Visibility = Visibility.Hidden;
                White_Turn.Visibility = Visibility.Visible;
                lblWhite_Turn.Visibility = Visibility.Visible;
            }
            else
            {
                Red_Turn.Visibility = Visibility.Visible;
                lblRed_Turn.Visibility = Visibility.Visible;
                White_Turn.Visibility = Visibility.Hidden;
                lblWhite_Turn.Visibility = Visibility.Hidden;
            }
        }
        private bool InitializeMultipleJumps()
        {
            if (lbl1SarituriMultiple.Content.ToString() == "False")
            {
                return false;
            }
            return true;
        }
        private void Window_Closing_1(object sender, EventArgs e)
        {
            FieldVM myFieldVM = this.DataContext as FieldVM;
            if (myFieldVM.RedPiecesNumberInGame != 0 && myFieldVM.WhitePiecesNumberInGame != 0)
            {
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Do you want to save the game ?","Save Changes", MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    myFieldVM.MemoriseCurrentState("CurrentState.txt");
                }
            }
        }
        private void VerifyMoves(FieldVM fieldVM)
        {
            if (fieldVM.RedPiecesNumberInGame < fieldVM.WhitePiecesNumberInGame)
            {
                EndGame(Piece.PieceCol.WHITE, lblPlayer1Name.Content.ToString());
            }
            else if (fieldVM.RedPiecesNumberInGame > fieldVM.WhitePiecesNumberInGame)
            {
                EndGame(Piece.PieceCol.RED, lblPlayer2Name.Content.ToString());
            }
            else
            {
                if (fieldVM.CurrentField.FieldPiece.Color == Piece.PieceCol.RED)
                {
                    EndGame(Piece.PieceCol.RED, lblPlayer2Name.Content.ToString());
                }
                else
                {
                    EndGame(Piece.PieceCol.WHITE, lblPlayer1Name.Content.ToString());
                }
            }
        }
        private void EndGame(Piece.PieceCol winnerCol, string winnerName)
        {
            DameGameOver dmgo = new DameGameOver();
            dmgo.lblWinnerName.Content = (winnerName + " - ");
            dmgo.lblWinnerColor.Content = winnerCol;
            this.Close();
            dmgo.Show();
        }
    }
}

