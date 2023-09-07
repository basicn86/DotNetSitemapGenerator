using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSitemapGenerator.ViewModels.MainWindow
{
    public class CurrentOperationViewModel : INotifyPropertyChanged
    {
        //create event handler
        public event PropertyChangedEventHandler? PropertyChanged;

        //raise the event
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string? _CurrentOperation;

        public string? CurrentOperation
        {
            get
            {
                return _CurrentOperation;
            }

            set
            {
                if (_CurrentOperation != value)
                {
                    _CurrentOperation = value;

                    RaisePropertyChanged();
                }
            }
        }

        public CurrentOperationViewModel()
        {
            CurrentOperation = "Ready";
        }
    }
}
