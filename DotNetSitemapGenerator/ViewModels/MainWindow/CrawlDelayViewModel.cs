using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DotNetSitemapGenerator.ViewModels.MainWindow
{
    public class CrawlDelayViewModel : INotifyPropertyChanged
    {
        //event handler
        public event PropertyChangedEventHandler? PropertyChanged;

        //raise the event
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _CrawlDelay;

        public object CrawlDelay
        {
            get { return _CrawlDelay; }
            set
            {
                if (int.TryParse(value.ToString(), out int result))
                {
                    if(result < 1) result = 1;
                    _CrawlDelay = result;
                    RaisePropertyChanged();
                }
                else
                {
                    _CrawlDelay = 1;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
