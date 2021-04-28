using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;

namespace CheckersGame
{
    class FieldVM : PropertyNotifyClass
    {
        private ObservableCollection<ObservableCollection<Field>> board;
        public bool jumpOverPiece { get; set; }
        private bool multipleJumps { get; set; }
        private Piece.PieceCol currentTurn;

        public bool MultipleJumps
        {
            get
            {
                return multipleJumps;
            }
            set
            {
                multipleJumps = value;
                NotifyPropertyChanged("MultipleJumps");
            }
        }
        public Piece.PieceCol CurrentTurn
        {
            get
            {
                return currentTurn;
            }
            set
            {
                currentTurn = value;
                NotifyPropertyChanged("CurrentTurn");
            }
        }
        private Field currentField;
        public Field CurrentField
        {
            get
            {
                return currentField;
            }

            set
            {
                currentField = value;
                NotifyPropertyChanged("CurrentField");
            }
        }
        private Field newPositionField;
        public Field NewPositionField
        {
            get
            {
                return newPositionField;
            }

            set
            {
                newPositionField = value;
                NotifyPropertyChanged("NewPositionField");
            }
        }

        public ObservableCollection<ObservableCollection<Field>> Board
        {
            get { return board; }
        }

        private void IncludeFolders_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("Board");
        }

        private int whitePiecesNumberInGame;

        public int WhitePiecesNumberInGame
        {
            get
            {
                return whitePiecesNumberInGame;
            }
            set
            {
                whitePiecesNumberInGame = value;
                NotifyPropertyChanged("WhitePiecesNumberInGame");
            }
        }

        private int redPiecesNumberInGame;

        public int RedPiecesNumberInGame
        {
            get
            {
                return redPiecesNumberInGame;
            }
            set
            {
                redPiecesNumberInGame = value;
                NotifyPropertyChanged("RedPiecesNumberInGame");
            }
        }

        public FieldVM()
        {
            redPiecesNumberInGame = 12;
            whitePiecesNumberInGame = 12;
            board = new ObservableCollection<ObservableCollection<Field>>();
            board.CollectionChanged += IncludeFolders_CollectionChanged;
            InitializeBoard(GameInitializationVM.ChooseSavedGame);
        }

        public void InitializeBoard(string filePathInitialBoard)
        {
            using (StreamReader file = new StreamReader(filePathInitialBoard))
            {
                int lineIndex = 0;
                string line;

                while (lineIndex < 8)
                {
                    line = file.ReadLine();
                    
                    ObservableCollection<Field> listLine = new ObservableCollection<Field>();
                    for (int columnIndex = 0; columnIndex < line.Length; columnIndex++)
                    {
                        Field myField = new Field();
                        myField.knowImageChooseField(line[columnIndex] - '0', lineIndex, columnIndex);
                        listLine.Add(myField);
                    }
                    board.Add(listLine);
                    lineIndex++;
                }
                line = file.ReadLine();
                currentTurn = new Piece().StringToPieceCol(line);
                file.Close();
            }
        }

        public bool ChangePiecePosition(int coordX, int coordY)
        {
            if (ValidateMove())
            {
                NewPositionField = new Field(CurrentField);
                NewPositionField.fieldCoordinates = new Tuple<int, int>(coordX, coordY);

                if ((NewPositionField.fieldCoordinates.Item1 == 0 && NewPositionField.FieldPiece.Color == Piece.PieceCol.RED ||
                    NewPositionField.fieldCoordinates.Item1 == 7 && NewPositionField.FieldPiece.Color == Piece.PieceCol.WHITE) &&
                    !NewPositionField.FieldPiece.IsKing)
                {
                    NewPositionField.MakeKing();
                }
                Board[NewPositionField.fieldCoordinates.Item1][NewPositionField.fieldCoordinates.Item2] = NewPositionField;

                CurrentField.FieldImagePath = "Images\\2_font_inch.jpg";
                int cfkey = CurrentField.fieldConfiguration.FirstOrDefault(x => x.Value == CurrentField.FieldImagePath).Key;
                CurrentField.knowImageChooseField(cfkey, CurrentField.fieldCoordinates.Item1, CurrentField.fieldCoordinates.Item2);
                
                Board[CurrentField.fieldCoordinates.Item1][CurrentField.fieldCoordinates.Item2] = CurrentField;
                
                if (jumpOverPiece)
                {
                    APieceWasExtracted((CurrentField.fieldCoordinates.Item1 + NewPositionField.fieldCoordinates.Item1) / 2,
                                        (CurrentField.fieldCoordinates.Item2 + NewPositionField.fieldCoordinates.Item2) / 2);

                }
                else
                {
                    CurrentField = null;
                    NewPositionField = null;
                }
                return true;
            }
            return false;
        }

        public bool ValidateMove()
        {
            if (newPositionField.FieldPiece != null)
            {
                return false;
            }

            int positionXcurrentPiece = currentField.fieldCoordinates.Item1, positionYcurrentPiece = currentField.fieldCoordinates.Item2;
            int newPositionXPiece = newPositionField.fieldCoordinates.Item1, newPositionYPiece = newPositionField.fieldCoordinates.Item2;
            int step;

            if (currentField.FieldPiece.Color == Piece.PieceCol.RED)
            {
                step = 1;
            }
            else
            {
                step = -1;
            }
            if (!jumpOverPiece)
            {
                if (positionXcurrentPiece - step == newPositionXPiece &&
                (positionYcurrentPiece - 1 == newPositionYPiece || positionYcurrentPiece + 1 == newPositionYPiece) ||
                currentField.FieldPiece.IsKing && positionXcurrentPiece + step == newPositionXPiece &&
                (positionYcurrentPiece - 1 == newPositionYPiece || positionYcurrentPiece + 1 == newPositionYPiece))
                {
                    return true;
                }
            }

            return ValidateMove2Steps(step, newPositionField.fieldCoordinates.Item1, newPositionField.fieldCoordinates.Item2);
        }

        public bool ValidateMove2Steps(int step, int newPositionXPiece, int newPositionYPiece)
        {
            if (newPositionXPiece > -1 && newPositionXPiece < 8 && newPositionYPiece > -1 && newPositionYPiece < 8)
            {
                if (board[newPositionXPiece][newPositionYPiece].FieldPiece != null)
                {
                    return false;
                }

                int positionXcurrentPiece = currentField.fieldCoordinates.Item1, positionYcurrentPiece = currentField.fieldCoordinates.Item2;
                jumpOverPiece = false;

                if (positionXcurrentPiece - step < 8 && positionXcurrentPiece - step > -1)
                {
                    if (positionYcurrentPiece - 1 > -1)
                    {
                        if (board[positionXcurrentPiece - step][positionYcurrentPiece - 1].FieldPiece != null &&
                        positionXcurrentPiece - 2 * step == newPositionXPiece && positionYcurrentPiece - 2 == newPositionYPiece)
                            if (board[positionXcurrentPiece - step][positionYcurrentPiece - 1].FieldPiece.Color != currentField.FieldPiece.Color)
                            {
                                jumpOverPiece = true;
                                return true;
                            }
                    }
                    if (positionYcurrentPiece + 1 < 8)
                    {
                        if (board[positionXcurrentPiece - step][positionYcurrentPiece + 1].FieldPiece != null &&
                        positionXcurrentPiece - 2 * step == newPositionXPiece && positionYcurrentPiece + 2 == newPositionYPiece)
                            if (board[positionXcurrentPiece - step][positionYcurrentPiece + 1].FieldPiece.Color != currentField.FieldPiece.Color)
                            {
                                jumpOverPiece = true;
                                return true;
                            }
                    }
                }
                if (positionXcurrentPiece + step < 8 && positionXcurrentPiece + step > -1)
                {
                    if (positionYcurrentPiece - 1 > -1)
                    {

                        if (board[positionXcurrentPiece + step][positionYcurrentPiece - 1].FieldPiece != null &&
                        currentField.FieldPiece.IsKing && positionXcurrentPiece + 2 * step == newPositionXPiece &&
                        positionYcurrentPiece - 2 == newPositionYPiece)
                        {
                            if (board[positionXcurrentPiece + step][positionYcurrentPiece - 1].FieldPiece.Color != currentField.FieldPiece.Color)
                            {
                                jumpOverPiece = true;
                                return true;
                            }
                        }
                    }
                    if (positionYcurrentPiece + 1 < 8)
                    {
                        if (board[positionXcurrentPiece + step][positionYcurrentPiece + 1].FieldPiece != null &&
                        currentField.FieldPiece.IsKing && positionXcurrentPiece + 2 * step == newPositionXPiece &&
                        positionYcurrentPiece + 2 == newPositionYPiece)
                        {
                            if (board[positionXcurrentPiece + step][positionYcurrentPiece + 1].FieldPiece.Color != currentField.FieldPiece.Color)
                            {
                                jumpOverPiece = true;
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        
        public bool NoMoreMovesLeft(Piece.PieceCol pieceCol)
        {
            foreach (ObservableCollection<Field> line in board)
            {
                foreach (Field field in line)
                {
                    if (field.FieldPiece != null)
                    {
                        if (field.FieldPiece.Color == pieceCol && PieceCanBeMoved(field))
                            return true;
                    }
                }
            }
            return false;
        }
        public bool PieceCanBeMoved(Field field, bool justJumps = false)
        {
            int step;
            if (field.FieldPiece.Color == Piece.PieceCol.RED)
            {
                step = 1;
            }
            else
            {
                step = -1;
            }
            if (!justJumps)
            {
                if (field.fieldCoordinates.Item2 + 1 < 8 && field.fieldCoordinates.Item2 - 1 > -1)
                {
                    if (field.fieldCoordinates.Item1 - step < 8 && field.fieldCoordinates.Item1 - step > -1)
                    {
                        if (board[field.fieldCoordinates.Item1 - step][field.fieldCoordinates.Item2 - 1].FieldPiece == null)
                            return true;
                        if (board[field.fieldCoordinates.Item1 - step][field.fieldCoordinates.Item2 + 1].FieldPiece == null)
                            return true;
                    }
                    if (field.FieldPiece.IsKing && field.fieldCoordinates.Item1 + step < 8 && field.fieldCoordinates.Item1 + step > -1)
                    {
                        if (board[field.fieldCoordinates.Item1 + step][field.fieldCoordinates.Item2 - 1].FieldPiece == null)
                            return true;
                        if (board[field.fieldCoordinates.Item1 + step][field.fieldCoordinates.Item2 + 1].FieldPiece == null)
                            return true;
                    }
                }
            }
            if (ValidateMove2Steps(step, field.fieldCoordinates.Item1 - 2 * step, field.fieldCoordinates.Item2 - 2) ||
             ValidateMove2Steps(step, field.fieldCoordinates.Item1 - 2 * step, field.fieldCoordinates.Item2 + 2) ||
             (field.FieldPiece.IsKing && (ValidateMove2Steps(step, field.fieldCoordinates.Item1 + 2 * step, field.fieldCoordinates.Item2 - 2) ||
             ValidateMove2Steps(step, field.fieldCoordinates.Item1 + 2 * step, field.fieldCoordinates.Item2 + 2))))
                return true;

            return false;
        }
        public void APieceWasExtracted(int coordXExtractedPiece, int coordYExtractedPiece)
        {
            if (board[coordXExtractedPiece][coordYExtractedPiece].FieldPiece.Color == Piece.PieceCol.RED)
            {
                redPiecesNumberInGame--;
                NotifyPropertyChanged("RedPiecesNumberInGame");
            }
            else
            {
                whitePiecesNumberInGame--;
                NotifyPropertyChanged("WhitePiecesNumberInGame");
            }
            board[coordXExtractedPiece][coordYExtractedPiece].FieldImagePath = board[coordXExtractedPiece][coordYExtractedPiece].fieldConfiguration[2];
            board[coordXExtractedPiece][coordYExtractedPiece].knowImageChooseField(2, coordXExtractedPiece, coordYExtractedPiece);
        }

        public void MemoriseCurrentState(string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (ObservableCollection<Field> line in board)
                {
                    foreach (Field field in line)
                    {
                        sw.Write(field.fieldConfiguration.FirstOrDefault(x => x.Value == field.FieldImagePath).Key);
                    }
                    sw.Write("\n");
                }
                sw.Write(currentTurn.ToString());
                sw.Close();
            }
        }
    }
}
