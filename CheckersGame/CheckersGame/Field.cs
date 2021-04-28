using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckersGame
{
    class Field : PropertyNotifyClass
    {
        public enum FieldCol
        {
            LIGHT,
            DARK
        }
        private FieldCol fieldColor;
        private Piece fieldPiece;
        private string fieldImagePath;
        public Tuple<int, int> fieldCoordinates { get; set; }
        public Dictionary<int, string> fieldConfiguration = new Dictionary<int, string>();


        public FieldCol FieldColor
        {
            get
            {
                return fieldColor;
            }
            set
            {
                fieldColor = value;
                NotifyPropertyChanged("FieldColor");
            }
        }

        public Piece FieldPiece
        {
            get
            {
                return fieldPiece;
            }
            set
            {
                fieldPiece = value;
                NotifyPropertyChanged("FieldPiece");
            }
        }

        public string FieldImagePath
        {
            get
            {
                return fieldImagePath;
            }
            set
            {
                fieldImagePath = value;
                NotifyPropertyChanged("FieldImagePath");
            }
        }


        public Field()
        {
            fieldPiece = null;
            fieldColor = FieldCol.LIGHT;
            initializeDictionary();
            knowFieldChooseImage();
        }
        public Field(Field other)
        {
            fieldCoordinates = other.fieldCoordinates;
            fieldPiece = other.fieldPiece;
            fieldColor = other.fieldColor;
            initializeDictionary();
            knowFieldChooseImage();
        }

        public void initializeDictionary()
        {
            fieldConfiguration.Add(1, "Images\\1_font_desch.jpg");
            fieldConfiguration.Add(2, "Images\\2_font_inch.jpg");
            fieldConfiguration.Add(3, "Images\\3_font_inch_piesa_alba.jpg");
            fieldConfiguration.Add(4, "Images\\4_font_inch_piesa_alba_king.jpg");
            fieldConfiguration.Add(5, "Images\\5_font_inch_piesa_rosie.jpg");
            fieldConfiguration.Add(6, "Images\\6_font_inch_piesa_rosie_king.jpg");
        }
        public Field(Tuple<int, int> fieldcoordinates, FieldCol fieldcolor, Piece piece)
        {
            fieldCoordinates = fieldcoordinates;
            fieldColor = fieldcolor;
            fieldPiece = piece;
            initializeDictionary();
            knowFieldChooseImage();
        }

        public void knowFieldChooseImage()
        {
            if (fieldColor == FieldCol.LIGHT)
            {
                fieldImagePath = fieldConfiguration[1];
                return;
            }
            if (fieldPiece == null)
            {
                fieldImagePath = fieldConfiguration[2];
            }
            else if (fieldPiece.Color == Piece.PieceCol.WHITE)
            {
                if (fieldPiece.IsKing)
                {
                    fieldImagePath = fieldConfiguration[4];
                }
                else
                {
                    fieldImagePath = fieldConfiguration[3];
                }
            }
            else
            {
                if (fieldPiece.IsKing)
                {
                    fieldImagePath = fieldConfiguration[6];
                }
                else
                {
                    fieldImagePath = fieldConfiguration[5];
                }
            }
            NotifyPropertyChanged("FieldImagePath");
        }

        public void knowImageChooseField(int fieldConf, int lineIndexNo, int columnIndexNo)
        {
            switch (fieldConf)
            {
                case 1:
                    fieldColor = FieldCol.LIGHT;
                    fieldPiece = null;
                    break;

                case 2:
                    fieldColor = FieldCol.DARK;
                    fieldPiece = null;
                    break;
                case 3:
                    fieldColor = FieldCol.DARK;
                    fieldPiece = new Piece();
                    fieldPiece.Color = Piece.PieceCol.WHITE;
                    fieldPiece.IsKing = false;
                    break;
                case 4:
                    fieldColor = FieldCol.DARK;
                    fieldPiece = new Piece();
                    fieldPiece.Color = Piece.PieceCol.WHITE;
                    fieldPiece.IsKing = true;
                    break;
                case 5:
                    fieldColor = FieldCol.DARK;
                    fieldPiece = new Piece();
                    fieldPiece.Color = Piece.PieceCol.RED;
                    fieldPiece.IsKing = false;
                    break;
                case 6:
                    fieldColor = FieldCol.DARK;
                    fieldPiece = new Piece();
                    fieldPiece.Color = Piece.PieceCol.RED;
                    fieldPiece.IsKing = true;
                    break;
                default: break;

            };
            fieldCoordinates = new Tuple<int, int>(lineIndexNo, columnIndexNo);
            fieldImagePath = fieldConfiguration[fieldConf];
            NotifyPropertyChanged("FieldImagePath");
        }

        public string FieldColToString(Field.FieldCol color)
        {
            if (color == FieldCol.DARK)
            {
                return "dark";
            }
            return "light";
        }

        public void ShowPropreties()
        {
            Console.Write(@"Field coordinates: ({0},{1})", fieldCoordinates.Item1, fieldCoordinates.Item2);
            Console.Write("\nField Color: " + FieldColToString(FieldColor));
            if (fieldPiece != null)
            {
                Console.Write("\nPiece Color: " + FieldPiece.PieceColToString(fieldPiece.Color) + "\nIs king: " + FieldPiece.IsKing + "\n");

            }
        }

        public void MakeKing()
        {
            int index = fieldConfiguration.FirstOrDefault(x => x.Value == FieldImagePath).Key;
            FieldImagePath = fieldConfiguration[++index];
            knowImageChooseField(index, fieldCoordinates.Item1, fieldCoordinates.Item2);
            
        }

    }
}
