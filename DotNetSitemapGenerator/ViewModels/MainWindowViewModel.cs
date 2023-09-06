using DotNetSitemapGenerator.Utilities;
using System;
using System.Threading.Tasks;

namespace DotNetSitemapGenerator.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string RequestedURL { get; set; } = "http://dev.localhost";
        public CurrentOperationViewModel CurrentOperationViewModel { get; } = new CurrentOperationViewModel();

        public async void ButtonClick()
        {
            CurrentOperationViewModel.CurrentOperation = "Downloading...";
            try
            {
                WebPage page = await HttpDownloader.DownloadStringAsync(new Uri(RequestedURL));
                CurrentOperationViewModel.CurrentOperation = "Finished!" + RequestedURL;
            } catch (Exception ex)
            {
                CurrentOperationViewModel.CurrentOperation = "Error: " + ex.Message;
            }
        }
    }
}