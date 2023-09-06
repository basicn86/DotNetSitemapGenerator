using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSitemapGenerator.ViewModels
{
    public class CurrentProgessViewModel : INotifyPropertyChanged
    {
        //event handler for property changed
        public event PropertyChangedEventHandler? PropertyChanged;

        private int _CurrentProgress = 0;

        //raise the event
        private void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //public property
        public int CurrentProgress
        {
            get
            {
                return _CurrentProgress;
            }

            set
            {
                if (_CurrentProgress != value)
                {
                    _CurrentProgress = value;

                    RaisePropertyChanged();
                }
            }
        }

        //on construct, set the property to 0
        public CurrentProgessViewModel()
        {
            CurrentProgress = 0;
        }
    }
}
