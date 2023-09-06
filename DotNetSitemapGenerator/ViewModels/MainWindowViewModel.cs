using DotNetSitemapGenerator.Utilities;
using System;
using System.Threading.Tasks;
using Avalonia.Dialogs;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.IO;
using System.Linq;

namespace DotNetSitemapGenerator.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string RequestedURL { get; set; } = "http://dev.localhost";
        private CurrentOperationViewModel _CurrentOperationViewModel { get; }
        private SaveDirectoryViewModel _SaveDirectoryViewModel { get; }

        public CurrentOperationViewModel CurrentOperationViewModel
        {
            get { return _CurrentOperationViewModel; }
        }
        public SaveDirectoryViewModel SaveDirectoryViewModel
        {
            get { return _SaveDirectoryViewModel; }
        }

        public MainWindowViewModel()
        {
            _CurrentOperationViewModel = new CurrentOperationViewModel();
            _SaveDirectoryViewModel = new SaveDirectoryViewModel();
        }

        public async void ButtonClick()
        {
            CurrentOperationViewModel.CurrentOperation = "Downloading...";
            try
            {
                WebPage page = await HttpDownloader.DownloadStringAsync(new Uri(RequestedURL));
                CurrentOperationViewModel.CurrentOperation = "Finished downloading!";

                CurrentOperationViewModel.CurrentOperation = "Saving file...";
                StreamWriter writer = new StreamWriter(SaveDirectoryViewModel.SaveDirectory, false);
                await writer.WriteAsync(page.Content);
                writer.Close();
                CurrentOperationViewModel.CurrentOperation = "File saved!";
            } catch (Exception ex)
            {
                CurrentOperationViewModel.CurrentOperation = "Error: " + ex.Message;
            }
        }

        public async void SaveFile()
        {
            var dialog = new SaveFileDialog()
            {
                Title = "Save Sitemap",
                InitialFileName = "sitemap.xml",
                DefaultExtension = "xml"
            };

            //open the dialog
            var result = await dialog.ShowAsync(new Window());

            if (result != null)
            {
                SaveDirectoryViewModel.SaveDirectory = result;
                CurrentOperationViewModel.CurrentOperation = "Saving to " + result;
            }
        }
    }
}