using System.Collections.ObjectModel;
using System.IO;

namespace CheckersGame
{
    class AboutClass : PropertyNotifyClass
    {
        private ObservableCollection<string> textAbout;
        public ObservableCollection<string> TextAbout
        {
            get { return textAbout; }
        }

        public AboutClass()
        {
            UploadFile("AboutText.txt");
        }

        private void UploadFile(string FilePath)
        {
            textAbout = new ObservableCollection<string>();
            using (StreamReader sr = new StreamReader(FilePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    textAbout.Add(line);
                }
                sr.Close();
            }
        }
    }
}
