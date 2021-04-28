using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame
{
    class Piece : PropertyNotifyClass
    {
        public enum PieceCol
        {
            WHITE,
            RED
        }
        private PieceCol color;

        public PieceCol Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
                NotifyPropertyChanged("Color");
            }
        }
        private bool isKing;
        public bool IsKing
        {
            get
            {
                return isKing;
            }
            set
            {
                isKing = value;
                NotifyPropertyChanged("IsKing");
            }

        }

        public string PieceColToString(PieceCol color)
        {
            if (color == PieceCol.RED)
            {
                return "red";
            }
            return "white";
        }
        public PieceCol StringToPieceCol(string color)
        {
            if (color.ToUpper() == "RED")
            {
                return PieceCol.RED;
            }
            return PieceCol.WHITE;
        }
    }
}
