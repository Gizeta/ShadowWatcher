using ShadowWatcher.Socket;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;

namespace ShadowWatcher
{
    public class SettingModel : INotifyPropertyChanged
    {
        private IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForAssembly();

        public SettingModel()
        {
            if (!storage.FileExists("setting"))
            {
                Save();
            }
            else
            {
                var reader = new StreamReader(new IsolatedStorageFileStream("setting", FileMode.Open, storage));
                Settings.Parse(reader.ReadLine());
            }
        }

        public bool ShowSummonCard
        {
            get => Settings.ShowSummonCard;
            set
            {
                if (Settings.ShowSummonCard != value)
                {
                    Settings.ShowSummonCard = value;
                    Save();
                    NotifyPropertyChanged("ShowSummonCard");
                }
            }
        }

        public void Save()
        {
            var writer = new StreamWriter(new IsolatedStorageFileStream("setting", FileMode.Create, storage));
            writer.Write(Settings.ToString());
            writer.Close();

            Sender.Send($"Setting:{Settings.ToString()}");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
