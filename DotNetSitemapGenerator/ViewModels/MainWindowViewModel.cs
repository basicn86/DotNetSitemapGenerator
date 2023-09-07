using DotNetSitemapGenerator.Utilities;
using System;
using System.Threading.Tasks;
using Avalonia.Dialogs;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using DotNetSitemapGenerator.ViewModels.MainWindow;

namespace DotNetSitemapGenerator.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private int MaxDepth = 1200;
        private CancellationTokenSource? _CancellationTokenSource;
        public string RequestedURL { get; set; } = "http://books.localhost";
        #region ViewModels
        private CurrentOperationViewModel _CurrentOperationViewModel { get; }
        private SaveDirectoryViewModel _SaveDirectoryViewModel { get; }
        private CurrentProgessViewModel _CurrentProgessViewModel { get; }
        private MaxDepthViewModel _MaxDepthViewModel { get; }

        public MaxDepthViewModel MaxDepthViewModel
        {
            get { return _MaxDepthViewModel; }
        }
        public CurrentOperationViewModel CurrentOperationViewModel
        {
            get { return _CurrentOperationViewModel; }
        }
        public SaveDirectoryViewModel SaveDirectoryViewModel
        {
            get { return _SaveDirectoryViewModel; }
        }

        public CurrentProgessViewModel CurrentProgessViewModel
        {
            get { return _CurrentProgessViewModel; }
        }

        #endregion
        public MainWindowViewModel()
        {
            _CurrentOperationViewModel = new CurrentOperationViewModel();
            _SaveDirectoryViewModel = new SaveDirectoryViewModel();
            _CurrentProgessViewModel = new CurrentProgessViewModel();
            _MaxDepthViewModel = new MaxDepthViewModel();
        }

        public async void StartGenerating()
        {
            _CancellationTokenSource = new CancellationTokenSource();

            CurrentOperationViewModel.CurrentOperation = "Starting...";
            List<Uri> goodUris = new List<Uri>();
            List<Uri> badUris = new List<Uri>();
            Queue<Uri> queuedUris = new Queue<Uri>();

            MaxDepth = (int)MaxDepthViewModel.MaxDepth;

            try
            {
                //if the save directory is null or empty, then throw an exception
                if (string.IsNullOrEmpty(SaveDirectoryViewModel.SaveDirectory)) throw new Exception("Save directory cannot be null or empty");

                WebPage mainPage = await HttpDownloader.DownloadPageAsync(new Uri(RequestedURL));

                goodUris.Add(mainPage.Uri);

                //parse the html
                HtmlParser parser = new HtmlParser(mainPage);

                //get the links from the parser
                List<Uri> links = parser.GetLinks();
                foreach (Uri uri in links)
                {
                    //if the uri is already enqueued, do not enqueue it again
                    if (queuedUris.Contains(uri) || goodUris.Contains(uri)) continue;
                    queuedUris.Enqueue(uri);
                }

                while (goodUris.Count < MaxDepth && queuedUris.Count != 0)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(1));
                    _CancellationTokenSource.Token.ThrowIfCancellationRequested();

                    Uri uri = queuedUris.Dequeue();

                    //update the current operation to show the current uri
                    CurrentOperationViewModel.CurrentOperation = "Downloading " + uri.ToString();

                    try
                    {
                        WebPage page = await HttpDownloader.DownloadPageAsync(uri);
                        goodUris.Add(page.Uri);

                        //if the page was a redirect add "uri" to the bad uri list
                        if (page.Redirected) badUris.Add(uri);

                        //parse the html
                        parser = new HtmlParser(page);

                        links = parser.GetLinks();
                        foreach (Uri _uri in links)
                        {
                            //if the uri is already in any of the three lists, then skip it
                            if (goodUris.Contains(_uri) || queuedUris.Contains(_uri) || badUris.Contains(_uri)) continue;
                            //if it does not exist, then add it to the queued uris
                            queuedUris.Enqueue(_uri);
                        }
                    } catch (Exception ex)
                    {
                        badUris.Add(uri); //bad URIs get sent to this list
                    }

                    //update the progress
                    CurrentProgessViewModel.CurrentProgress = (int)((float)goodUris.Count / (float)MaxDepth * 100f);
                }

                CurrentProgessViewModel.CurrentProgress = 100;
                CurrentOperationViewModel.CurrentOperation = "Generating sitemap";

                await GenerateSiteMapAsync(goodUris);

                //Finished
                _CancellationTokenSource = null;
                CurrentOperationViewModel.CurrentOperation = "Done!";
            } catch (Exception ex)
            {
                _CancellationTokenSource = null;
                CurrentOperationViewModel.CurrentOperation = "Error: " + ex.Message;
            }
        }

        private async Task GenerateSiteMapAsync(List<Uri> uris)
        {
            //generate the sitemap
            string sitemap = SitemapGenerator.GenerateSitemap(uris);
            //save it to a file asynchronously
            CurrentOperationViewModel.CurrentOperation = "Saving sitemap";
            StreamWriter writer = new StreamWriter(SaveDirectoryViewModel.SaveDirectory);
            await writer.WriteAsync(sitemap);
            writer.Close();
        }

        public void CancelGenerating()
        {
            _CancellationTokenSource?.Cancel();
        }

        public async void SetSaveFile()
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