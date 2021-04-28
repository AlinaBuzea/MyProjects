using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame
{
    class GameInitializationVM : PropertyNotifyClass
    {
        private string namePlayerRedPieces;

        public string NamePlayerRedPieces
        {
            get
            {
                return namePlayerRedPieces;
            }
            set
            {
                namePlayerRedPieces = value;
                NotifyPropertyChanged("NamePlayerRedPieces");
            }
        }
        private string namePlayerWhitePieces;
        public string NamePlayerWhitePieces
        {
            get
            {
                return namePlayerWhitePieces;
            }
            set
            {
                namePlayerWhitePieces = value;
                NotifyPropertyChanged("NamePlayerWhitePieces");
            }
        }

        private bool gimultipleJumps;
        public bool giMultipleJumps
        {
            get
            {
                return gimultipleJumps;
            }
            set
            {
                gimultipleJumps = value;
                NotifyPropertyChanged("giMultipleJumps");
            }
        }
        private static string chooseSavedGame;
        public static string ChooseSavedGame
        {
            get
            {
                return chooseSavedGame;
            }
            set
            {
                chooseSavedGame = value;
            }
        }
        public GameInitializationVM()
        {
            gimultipleJumps = false;
            namePlayerRedPieces = "Player2";
            namePlayerWhitePieces = "Player1";
            chooseSavedGame = "Initial_state.txt";
        }

    }
}
