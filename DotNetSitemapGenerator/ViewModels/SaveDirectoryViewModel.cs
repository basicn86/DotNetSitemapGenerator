using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSitemapGenerator.ViewModels
{
    public class SaveDirectoryViewModel : INotifyPropertyChanged
    {
        //create event handler
        public event PropertyChangedEventHandler? PropertyChanged;

        //raise the event
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string? _SaveDirectory;

        public string? SaveDirectory
        {
            get
            {
                return _SaveDirectory;
            }

            set
            {
                if (_SaveDirectory != value)
                {
                    _SaveDirectory = value;

                    RaisePropertyChanged();
                }
            }
        }

        public SaveDirectoryViewModel()
        {
            SaveDirectory = "";
        }
    }
}
