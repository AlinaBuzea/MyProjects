using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace CheckersGame
{
    class GameOverWinnersVM : PropertyNotifyClass
    {
        private ObservableCollection<string> evidenceOfWins;
        private string fileEvidenceName;
        public string winnerCol { get; set; }
        public ObservableCollection<string> EvidenceOfWins
        {
            get { return evidenceOfWins; }
        }
        private void IncludeFolders_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("EvidenceOfWins");
        }

        public GameOverWinnersVM()
        {
            fileEvidenceName = "NoOfWins.txt";
            InitializeCollection();
        }

        public void InitializeCollection()
        {
            evidenceOfWins = new ObservableCollection<string>();
            using (StreamReader sr = new StreamReader(fileEvidenceName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    evidenceOfWins.Add(line);
                }
                sr.Close();
            }
        }

        public void UpdateEvidence()
        {
            if (winnerCol != null)
            {
                if (winnerCol == "RED")
                {
                    evidenceOfWins[1] = (Int32.Parse(evidenceOfWins[1]) + 1).ToString();
                }
                else
                {
                    evidenceOfWins[3] = (Int32.Parse(evidenceOfWins[3]) + 1).ToString();
                }
                NotifyPropertyChanged("EvidenceOfWins");
                UpdateEvidenceFile();
            }
        }
        private void UpdateEvidenceFile()
        {
            using (StreamWriter sw = new StreamWriter(fileEvidenceName))
            {
                foreach (string line in evidenceOfWins)
                {
                    sw.WriteLine(line);
                }
                sw.Close();
            }
        }
    }
}


