using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSitemapGenerator.ViewModels
{
    public class MaxDepthViewModel : INotifyPropertyChanged
    {
        //event handler
        public event PropertyChangedEventHandler PropertyChanged;

        //private fields
        private int _MaxDepth = 1200;

        //raise event function
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //public properties
        public object MaxDepth
        {
            get { return _MaxDepth; }
            set
            {
                if (int.TryParse(value.ToString(), out int result))
                {
                    _MaxDepth = result;
                    RaisePropertyChanged();
                }
                else
                {
                    _MaxDepth = 0;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
